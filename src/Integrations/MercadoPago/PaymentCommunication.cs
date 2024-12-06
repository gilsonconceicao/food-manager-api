using Domain.Interfaces;
using Integrations.Settings;
using MercadoPago.Client.Common;
using MercadoPago.Client.Preference;
using MercadoPago.Config;
using MercadoPago.Resource.Preference;
using Microsoft.Extensions.Options;

namespace Integrations.MercadoPago;

public class PaymentCommunication : IPaymentCommunication
{
    private readonly IMercadoPagoClient _mercadoPagoClient;
    private readonly MercadoPagoSettings _mercadoPagoSettings;

    public PaymentCommunication(IOptions<MercadoPagoSettings> mercadoPagoSettings, IMercadoPagoClient mercadoPagoClient)
    {
        _mercadoPagoSettings = mercadoPagoSettings.Value;
        _mercadoPagoClient = mercadoPagoClient;
    }

    public async Task<Preference> CreatePixAsync()
    {
        _mercadoPagoClient.ConfigureAccessToken(_mercadoPagoSettings.AccessToken);
        MercadoPagoConfig.AccessToken = _mercadoPagoSettings.AccessToken;

        try
        {
            var request = new PreferenceRequest
            {
                Items = new List<PreferenceItemRequest>
                {
                    new PreferenceItemRequest
                    {
                        Id = "item-ID-1234",
                        Title = "Meu produto",
                        CurrencyId = "BRL",
                        PictureUrl = "https://www.mercadopago.com/org-img/MP3/home/logomp3.gif",
                        Description = "Descrição do Item",
                        CategoryId = "art",
                        Quantity = 1,
                        UnitPrice = 75.76m
                    }
                },
                Payer = new PreferencePayerRequest
                {
                    Name = "Gilson",
                    Surname = "Conceição Santos",
                    Email = "gilsonconceicaosantos.jr@gmail.com",
                    Phone = new PhoneRequest
                    {
                        AreaCode = "11",
                        Number = "958667970"
                    },
                    Identification = new IdentificationRequest
                    {
                        Type = "CPF",
                        Number = "54404459866"
                    }
                },
                // BackUrls = new PreferenceBackUrlsRequest
                // {
                //     Success = "https://www.success.com",
                //     Failure = "http://www.failure.com",
                //     Pending = "http://www.pending.com"
                // },
                AutoReturn = "approved",
                PaymentMethods = new PreferencePaymentMethodsRequest
                {
                    ExcludedPaymentMethods = new List<PreferencePaymentMethodRequest>(),
                    ExcludedPaymentTypes = new List<PreferencePaymentTypeRequest>(),
                },
                NotificationUrl = "https://webhook.site/87469150-e7c9-4024-b049-b216e97f6876",
                StatementDescriptor = "Bolos caseiros e variedades",
                ExternalReference = "Reference_1234",
                Expires = true,
                ExpirationDateFrom = DateTime.UtcNow,
                ExpirationDateTo = DateTime.UtcNow.AddMinutes(10)
            };

            var client = new PreferenceClient();
            Preference preference = await client.CreateAsync(request);

            return preference;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}
