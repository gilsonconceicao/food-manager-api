using System.Text.Json;
using Api.Services;
using Domain.Enums;
using Domain.Interfaces;
using Infrastructure.Database;
using Integrations.Settings;
using MercadoPago.Client.Common;
using MercadoPago.Client.Preference;
using MercadoPago.Config;
using MercadoPago.Resource.Preference;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Integrations.MercadoPago;

public class PaymentCommunication : IPaymentCommunication
{
    private readonly MercadoPagoSettings _mercadoPagoSettings;
    private readonly ICurrentUser _httpUserService;
    private readonly DataBaseContext _context;
    private readonly IMercadoPagoClient _mercadoPagoClient;


    public PaymentCommunication(
        IOptions<MercadoPagoSettings> mercadoPagoSettings,
        ICurrentUser httpUserService,
        DataBaseContext context,
        IMercadoPagoClient mercadoPagoClient
    )
    {
        _mercadoPagoSettings = mercadoPagoSettings.Value;
        _httpUserService = httpUserService;
        _context = context;
        _mercadoPagoClient = mercadoPagoClient; 
    }

    public async Task<Preference> CreateCheckoutProAsync(
        List<PreferenceItemRequest> items
    )
    {
        MercadoPagoConfig.AccessToken = _mercadoPagoSettings.AccessToken;

        try
        {
            var userAuthenticated = await _httpUserService.GetAuthenticatedUser();
            var user = _context
                .Users
                .FirstOrDefault(x => x.FirebaseUserId == userAuthenticated.UserId)
                ?? throw new Exception("Usuário autenticado não encontrado na base.");

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
                        Type = "PhoneNumber",
                        Number = user.PhoneNumber
                    },
                },
                AutoReturn = "approved",
                PaymentMethods = new PreferencePaymentMethodsRequest
                {
                    ExcludedPaymentMethods = new List<PreferencePaymentMethodRequest>(),
                    ExcludedPaymentTypes = new List<PreferencePaymentTypeRequest>()
                },
                BackUrls = new()
                {
                    Failure = "https://food-manager-one.vercel.app/comidas",
                    Pending = "https://food-manager-one.vercel.app/comidas",
                    Success = "https://food-manager-one.vercel.app/comidas"
                },
                StatementDescriptor = "Bolos e variedades da Cris",
                ExternalReference = $"UserId-{user.Id}-{user.Email}",
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
            throw new Exception($"Erro no processo ao gerar o link de pagamento: {ex.Message}", ex);
        }
    }

    public async Task VerifyPendingAsync()
    {
        var ordersPending = await _context.Orders
            .Where(o => o.Status == OrderStatus.AwaitingPayment)
            .ToListAsync();

        foreach (var order in ordersPending)
        {
            var status = await _mercadoPagoClient.GetPaymentStatusAsync(order.PaymentId);
            
            if (status == "approved")
            {
                order.Status = OrderStatus.Paid;
                _context.Orders.Update(order);
            }
        }

        await _context.SaveChangesAsync();
    }

    public async Task ProcessPaymentWebhookAsync(string paymentId)
{
    try
    {
        var accessToken = _mercadoPagoSettings.AccessToken;
        var response = await new HttpClient().GetAsync(
            $"https://api.mercadopago.com/v1/payments/{paymentId}?access_token={accessToken}");

        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        var json = JsonDocument.Parse(content).RootElement;

        var status = json.GetProperty("status").GetString();
        var externalReference = json.GetProperty("external_reference").GetString(); 

        if (string.IsNullOrEmpty(externalReference))
            return;

        var userIdStr = externalReference.Split('-').ElementAtOrDefault(1);
        if (!Guid.TryParse(userIdStr, out var userId))
            return;

        var order = await _context.Orders
            .FirstOrDefaultAsync(x => x.UserId == userId && x.Status == OrderStatus.AwaitingPayment);

        if (order == null) return;

        if (status == "approved")
        {
            order.Status = OrderStatus.Paid;
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Erro no webhook: {ex.Message}");
    }
}
}
