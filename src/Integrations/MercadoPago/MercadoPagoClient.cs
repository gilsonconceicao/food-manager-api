using System.Text.Json;
using Integrations.Settings;
using MercadoPago.Client.MerchantOrder;
using MercadoPago.Config;
using MercadoPago.Resource.MerchantOrder;
using MercadoPago.Resource.Preference;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
namespace Integrations.MercadoPago;

public interface IMercadoPagoClient
{
    Task<Preference?> GetPreferenceByIdAsync(string preferenceId);
    Task<MercadoPagoPaymentResult?> GetPaymentByIdAsync(string paymentId);
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

    public async Task<Preference?> GetPreferenceByIdAsync(string preferenceId)
    {
        var accessToken = _mercadoPagoSettings.AccessToken;
        var response = await _httpClient.GetAsync(
            $"https://api.mercadopago.com/checkout/preferences/{preferenceId}?access_token={accessToken}");

        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            _logger.LogError("❌ Erro ao buscar preferência {id} | Status: {status} | Conteúdo: {content}",
                             preferenceId, response.StatusCode, errorContent);
            return null;
        }

        var content = await response.Content.ReadAsStringAsync();
        var json = JsonDocument.Parse(content).RootElement;

        var preference = new Preference
        {
            Id = GetStringSafe(json, "id"),
            InitPoint = GetStringSafe(json, "init_point"),
            SandboxInitPoint = GetStringSafe(json, "sandbox_init_point"),
            NotificationUrl = GetStringSafe(json, "notification_url"),
            StatementDescriptor = GetStringSafe(json, "statement_descriptor"),
            ExternalReference = GetStringSafe(json, "external_reference"),
            Marketplace = GetStringSafe(json, "marketplace"),
            Purpose = GetStringSafe(json, "purpose"),
            AdditionalInfo = GetStringSafe(json, "additional_info"),
            AutoReturn = GetStringSafe(json, "auto_return"),
            OperationType = GetStringSafe(json, "operation_type"),
            DateOfExpiration = GetDateTimeSafe(json, "date_of_expiration"),
            ExpirationDateFrom = GetDateTimeSafe(json, "expiration_date_from"),
            ExpirationDateTo = GetDateTimeSafe(json, "expiration_date_to"),
            DateCreated = GetDateTimeSafe(json, "date_created"),
            Expires = json.TryGetProperty("expires", out var expiresProp) && expiresProp.ValueKind == JsonValueKind.True ? true :
                      (expiresProp.ValueKind == JsonValueKind.False ? false : (bool?)null),
            BinaryMode = json.TryGetProperty("binary_mode", out var binaryProp) && binaryProp.ValueKind == JsonValueKind.True ? true :
                         (binaryProp.ValueKind == JsonValueKind.False ? false : (bool?)null),
            CollectorId = json.TryGetProperty("collector_id", out var collectorProp) && collectorProp.TryGetInt64(out var collectorId) ? collectorId : (long?)null,
            SponsorId = json.TryGetProperty("sponsor_id", out var sponsorProp) && sponsorProp.TryGetInt64(out var sponsorId) ? sponsorId : (long?)null,
            MarketplaceFee = json.TryGetProperty("marketplace_fee", out var feeProp) && feeProp.TryGetDecimal(out var fee) ? fee : (decimal?)null,
            ProcessingModes = json.TryGetProperty("processing_modes", out var procProp) && procProp.ValueKind == JsonValueKind.Array
                ? procProp.EnumerateArray().Select(p => p.GetString()).Where(p => p != null).ToList()
                : new List<string>()
        };

        return preference;
    }
    string? GetStringSafe(JsonElement json, string propertyName)
    { 
        return json.TryGetProperty(propertyName, out var prop) && prop.ValueKind != JsonValueKind.Null
            ? prop.GetString()
            : null;
    }

    DateTime? GetDateTimeSafe(JsonElement json, string propertyName)
    {
        return json.TryGetProperty(propertyName, out var prop) && prop.ValueKind == JsonValueKind.String
            && DateTime.TryParse(prop.GetString(), out var date)
            ? date
            : null;
    }

}

public class MercadoPagoPaymentResult
{
    public string? PaymentId { get; set; }
    public string? Status { get; set; }
    public string? ExternalReference { get; set; }
    public string? StatusDetail { get; set; }
}