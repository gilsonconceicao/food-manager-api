using Api.Services;
using Domain.Interfaces;
using Infrastructure.Database;
using Integrations.Settings;
using MercadoPago.Client.Common;
using MercadoPago.Client.Preference;
using MercadoPago.Config;
using MercadoPago.Resource.Preference;
using Microsoft.Extensions.Options;

namespace Integrations.MercadoPago;

public class PaymentCommunication : IPaymentCommunication
{
    private readonly MercadoPagoSettings _mercadoPagoSettings;
    private readonly IHttpUserService _httpUserService;
    private readonly DataBaseContext _context;


    public PaymentCommunication(
        IOptions<MercadoPagoSettings> mercadoPagoSettings,
        IHttpUserService httpUserService,
        DataBaseContext context
    )
    {
        _mercadoPagoSettings = mercadoPagoSettings.Value;
        _httpUserService = httpUserService;
        _context = context;
    }

    public async Task<Preference> CreateCheckoutProAsync(
        List<PreferenceItemRequest> items
        // string payerName,
        // string payerSurname,
        // string payerEmail,
        // string notificationUrl,
        // string statementDescriptor
    )
    {
        MercadoPagoConfig.AccessToken = _mercadoPagoSettings.AccessToken;

        try
        {
            var userAuthenticated = await _httpUserService.GetAuthenticatedUser();
            var user = _context
                .Users
                .FirstOrDefault(x => x.FirebaseUserId == userAuthenticated.UserId)
                ?? throw new Exception("Item no carrinho  não encontrada ou não existe.");
            
            // Cria o objeto da preferência
            var request = new PreferenceRequest
            {
                Items = items,
                Payer = new PreferencePayerRequest
                {
                    Name = user.Name,
                    Surname = user.Name,
                    Email = user.Email,
                    Identification = new IdentificationRequest
                    {
                        Type = "CPF",
                        Number = user.RegistrationNumber
                    },
                },
                AutoReturn = "approved",
                PaymentMethods = new PreferencePaymentMethodsRequest
                {
                    ExcludedPaymentMethods = new List<PreferencePaymentMethodRequest>(),
                    ExcludedPaymentTypes = new List<PreferencePaymentTypeRequest>()
                },
                StatementDescriptor = "Bolos e variedades da Cris",
                ExternalReference = $"UserId-{user.Id}-{user.Email}",
                Expires = true,
                ExpirationDateFrom =  DateTime.UtcNow,
                ExpirationDateTo = DateTime.UtcNow.AddMinutes(10)
            };

            // Cria a preferência usando o client
            var client = new PreferenceClient();
            Preference preference = await client.CreateAsync(request);

            return preference;
        }
        catch (Exception ex)
        {
            // Lançar uma exceção mais detalhada
            throw new Exception($"Erro ao criar a preferência: {ex.Message}", ex);
        }
    }
}
