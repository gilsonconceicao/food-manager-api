using System.Text;
using System.Text.Json;
using Domain.Common.Exceptions;
using Domain.Enums;
using Domain.Extensions;
using Domain.Models.Request;
using Integrations.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
namespace Integrations.MercadoPago;

public interface IMercadoPagoClient
{
    Task<MercadoPagoPaymentResult?> GetPaymentByIdAsync(string paymentId);
    Task<CardBrandResult> GetCardBrandAsync(string bin);
    Task<CreateCardTokenResult> CreateCardTokenAsync(CardDataRequestDto request);
}

public class MercadoPagoClient : IMercadoPagoClient
{
    private readonly MercadoPagoSettings _mercadoPagoSettings;
    private readonly HttpClient _httpClient;
    private readonly ILogger<MercadoPagoClient> _logger;


    public MercadoPagoClient(IOptions<MercadoPagoSettings> mercadoPagoSetting, HttpClient httpClient, ILogger<MercadoPagoClient> logger)
    {
        _mercadoPagoSettings = mercadoPagoSetting.Value;
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<CreateCardTokenResult> CreateCardTokenAsync(CardDataRequestDto request)
    {
        try
        {
            var accessToken = _mercadoPagoSettings.AccessToken;

            var payload = new
            {
                card_number = request.CardNumber,
                expiration_month = request.ExpirationMonth,
                expiration_year = request.ExpirationYear,
                security_code = request.Cvv,
                cardholder = new
                {
                    name = request.CardHolderNamenoCartão
                }
            };

            var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{_mercadoPagoSettings.BaseURL}/card_tokens?access_token={accessToken}", content);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error creating card token: {error}");
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            using var json = JsonDocument.Parse(responseContent);
            var token = json.RootElement.GetProperty("id").GetString();
            var bin = json.RootElement.GetProperty("first_six_digits").GetString();

            return new CreateCardTokenResult
            {
                Bin = bin, 
                Token = token
            };
        }
        catch (Exception ex)
        {
            var message = StringExtensions.ExtractMessage(ex.Message.ToString());

            throw new HttpResponseException(
                StatusCodes.Status400BadRequest,
                CodeErrorEnum.INVALID_BUSINESS_RULE.ToString(),
                message ?? "Cartão inválido"
            );
        }
    }

    public async Task<CardBrandResult> GetCardBrandAsync(string bin)
    {
        var accessToken = _mercadoPagoSettings.AccessToken;
        var url = $"https://api.mercadopago.com/v1/payment_methods/search?bin={bin}&site_id=MLB";

        using var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

        var response = await _httpClient.SendAsync(request);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            throw new Exception($"Error getting card brand: {error}");
        }

        var content = await response.Content.ReadAsStringAsync();
        using var json = JsonDocument.Parse(content);

        var results = json.RootElement.GetProperty("results");

        if (results.GetArrayLength() == 0)
            throw new Exception("Card brand not found.");

        var paymentMethod = results[0];
        var cardBrand = paymentMethod.GetProperty("name").GetString(); 
        var paymentMethodId = paymentMethod.GetProperty("id").GetString();

        return new CardBrandResult
        {
            CardBrand = cardBrand, 
            PaymentMethodId = paymentMethodId
        };
    }
    public async Task<MercadoPagoPaymentResult?> GetPaymentByIdAsync(string paymentId)
    {
        var accessToken = _mercadoPagoSettings.AccessToken;
        var response = await _httpClient.GetAsync(
            $"{_mercadoPagoSettings.BaseURL}/payments/{paymentId}?access_token={accessToken}");

        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            _logger.LogError("❌ Erro ao buscar pagamento {id} | Status: {status} | Conteúdo: {content}",
                             paymentId, response.StatusCode, errorContent);
            return null;
        }

        var content = await response.Content.ReadAsStringAsync();
        var json = JsonDocument.Parse(content).RootElement;

        if (!json.TryGetProperty("id", out var idProp) ||
            !json.TryGetProperty("status", out var statusProp) ||
            !json.TryGetProperty("external_reference", out var external_reference))
        {
            _logger.LogError($"Pagamento inválido ou incompleto: {content}");
            return null;
        }

        return new MercadoPagoPaymentResult
        {
            PaymentId = long.Parse(idProp.ToString()),
            Status = statusProp.GetString(),
            ExternalReference = external_reference.ToString(),
            StatusDetail = json.TryGetProperty("status_detail", out var detailProp) ? detailProp.GetString() : null
        };
    }
}

public class MercadoPagoPaymentResult
{
    public long? PaymentId { get; set; }
    public string? Status { get; set; }
    public string? ExternalReference { get; set; }
    public string? StatusDetail { get; set; }
}

public class CardBrandResult
{
    public string PaymentMethodId { get; set; }
    public string CardBrand { get; set; }
}

public class CreateCardTokenResult
{
    public string Token { get; set; }
    public string Bin { get; set; }
}