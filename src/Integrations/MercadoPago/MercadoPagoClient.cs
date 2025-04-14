using System.Text.Json;
using Integrations.Settings;
using MercadoPago.Client.MerchantOrder;
using MercadoPago.Config;
using MercadoPago.Resource.MerchantOrder;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
namespace Integrations.MercadoPago;

public interface IMercadoPagoClient
{
    Task<MercadoPagoPaymentResult?> GetPaymentByIdAsync(string preferenceId);
    Task<MerchantOrder> GetMerchantOrderByIdAsync(string merchantOrderId);
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

    public async Task<MercadoPagoPaymentResult?> GetPaymentByIdAsync(string paymentId)
    {
        var accessToken = _mercadoPagoSettings.AccessToken;
        var response = await _httpClient.GetAsync(
            $"https://api.mercadopago.com/v1/payments/{paymentId}?access_token={accessToken}");

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
            PaymentId = idProp.ToString(),
            Status = statusProp.GetString(),
            ExternalReference = external_reference.ToString(),
            StatusDetail = json.TryGetProperty("status_detail", out var detailProp) ? detailProp.GetString() : null
        };
    }

    public async Task<MerchantOrder> GetMerchantOrderByIdAsync(string merchantOrderId)
    {
        MercadoPagoConfig.AccessToken = _mercadoPagoSettings.AccessToken;

        var client = new MerchantOrderClient();
        var merchantOrder = await client.GetAsync(long.Parse(merchantOrderId));

        return merchantOrder;
    }


}

public class MercadoPagoPaymentResult
{
    public string? PaymentId { get; set; }
    public string? Status { get; set; }
    public string? ExternalReference { get; set; }
    public string? StatusDetail { get; set; }
}