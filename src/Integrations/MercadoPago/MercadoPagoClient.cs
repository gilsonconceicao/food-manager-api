using System.Text.Json;
using Integrations.Settings;
using MercadoPago.Config;
using Microsoft.Extensions.Options;
namespace Integrations.MercadoPago;

public interface IMercadoPagoClient
{
    Task<string> GetPaymentStatusAsync(string preferenceId);
}

public class MercadoPagoClient : IMercadoPagoClient
{
    private readonly MercadoPagoSettings _mercadoPagoSettings;
    private readonly HttpClient _httpClient;

    public MercadoPagoClient(IOptions<MercadoPagoSettings> mercadoPagoSetting, HttpClient httpClient)
    {
        _mercadoPagoSettings = mercadoPagoSetting.Value;
        _httpClient = httpClient;
    }

    public async Task<string> GetPaymentStatusAsync(string preferenceId)
    {
        MercadoPagoConfig.AccessToken = _mercadoPagoSettings.AccessToken;

        var response = await _httpClient.GetAsync(
            $"https://api.mercadopago.com/checkout/preferences/{preferenceId}?access_token={MercadoPagoConfig.AccessToken}");

        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        var json = JsonDocument.Parse(content);

        var status = json.RootElement
            .GetProperty("payments")
            .EnumerateArray()
            .FirstOrDefault()
            .GetProperty("status")
            .GetString();

        return status;
    }
}
