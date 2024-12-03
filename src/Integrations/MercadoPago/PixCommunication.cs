using Domain.Interfaces;
using Integrations.Settings;
using MercadoPago.Client;
using MercadoPago.Client.Common;
using MercadoPago.Client.Payment;
using MercadoPago.Config;
using MercadoPago.Resource.Payment;
using Microsoft.Extensions.Options;

namespace Integrations.MercadoPago;

public class PixCommunication : IPixCommunication
{
    private readonly IMercadoPagoClient _mercadoPagoClient;
    private readonly MercadoPagoSettings _mercadoPagoSettings;

    public PixCommunication(IOptions<MercadoPagoSettings> mercadoPagoSettings, IMercadoPagoClient mercadoPagoClient)
    {
        _mercadoPagoSettings = mercadoPagoSettings.Value;
        _mercadoPagoClient = mercadoPagoClient;
    }

    public async Task CreatePixAsync()
    {
        // Configura o access token do Mercado Pago
        _mercadoPagoClient.ConfigureAccessToken(_mercadoPagoSettings.AccessToken);
        MercadoPagoConfig.AccessToken = _mercadoPagoSettings.AccessToken;

        var requestOptions = new RequestOptions();
        requestOptions.CustomHeaders.Add("x-idempotency-key", "xpto");

        try
        {
            // Pedido com transação simplificada via PIX
            var request = new PaymentCreateRequest
            {
                TransactionAmount = 105,  // Valor da transação
                Description = "Título do produto",
                PaymentMethodId = "pix",  // Método de pagamento PIX
                Payer = new PaymentPayerRequest
                {
                    Email = "gilsonconceicaosantos.jr@gmail.com",  // E-mail do pagador
                    FirstName = "Gilson",  // Nome do pagador
                    LastName = "Conceição",  // Sobrenome do pagador
                    Identification = new IdentificationRequest
                    {
                        Type = "CPF",  // Tipo de identificação
                        Number = "544.044.598-66",  // CPF do pagador
                    }
                }
            };

            // Criação do pagamento
            var client = new PaymentClient();
            Payment payment = await client.CreateAsync(request, requestOptions);

            Console.WriteLine($"Pagamento PIX criado com sucesso. ID do pagamento: {payment.Id}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao criar pagamento PIX: {ex.Message}");
            throw;
        }

    }
}
