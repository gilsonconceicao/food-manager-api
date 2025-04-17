using Api.Services;
using Domain.Enums;
using Domain.Extensions;
using Domain.Interfaces;
using Domain.Models;
using Hangfire;
using Infrastructure.Database;
using Integrations.Settings;
using MercadoPago.Client.Common;
using MercadoPago.Client.Preference;
using MercadoPago.Config;
using MercadoPago.Resource.Preference;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Integrations.MercadoPago;

public class PaymentCommunication : IPaymentCommunication
{
    private readonly MercadoPagoSettings _mercadoPagoSettings;
    private readonly ICurrentUser _httpUserService;
    private readonly DataBaseContext _context;
    private readonly IMercadoPagoClient _mercadoPagoClient;
    private readonly ILogger<PaymentCommunication> _logger;


    public PaymentCommunication(
        IOptions<MercadoPagoSettings> mercadoPagoSettings,
        ICurrentUser httpUserService,
        DataBaseContext context,
        IMercadoPagoClient mercadoPagoClient,
        ILogger<PaymentCommunication> logger
    )
    {
        _mercadoPagoSettings = mercadoPagoSettings.Value;
        _httpUserService = httpUserService;
        _context = context;
        _mercadoPagoClient = mercadoPagoClient;
        _logger = logger;
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
                PaymentMethods = null,
                BackUrls = new()
                {
                    Failure = "https://food-manager-one.vercel.app/comidas",
                    Pending = "https://food-manager-one.vercel.app/comidas",
                    Success = "https://food-manager-one.vercel.app/comidas"
                },
                NotificationUrl = "https://f956-2804-7f0-455-136d-6cf5-e386-f166-7e5b.ngrok-free.app/webhooks/mercadopago",
                StatementDescriptor = "Bolos e variedades da Cris",
                ExternalReference = Guid.NewGuid().ToString(),
                Expires = true,
                ExpirationDateFrom = DateTime.UtcNow,
                ExpirationDateTo = DateTime.UtcNow.AddMinutes(5)
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

    public async Task<PaymentWebhookResult> ProcessPaymentWebhookAsync(string paymentId)
{
    try
    {
        var payment = await _mercadoPagoClient.GetPaymentByIdAsync(paymentId);

        _logger.LogInformation($"Processando webhook para PaymentId: {paymentId}");

        if (payment == null)
        {
            _logger.LogWarning($"Pagamento não encontrado para ID: {paymentId}");
            return PaymentWebhookResult.Fail("Pagamento não encontrado.");
        }

        var externalReference = payment.ExternalReference;
        bool isApproved = payment.Status == "approved";

        _logger.LogInformation($"Pagamento localizado: PreferenceId {externalReference}, Status {payment.Status}");

        var order = await _context.Orders
            .FirstOrDefaultAsync(o => o.ExternalPaymentId == externalReference);

        if (order == null)
        {
            _logger.LogWarning($"Pedido não encontrado para referência: {externalReference}");
            return PaymentWebhookResult.Fail("Pedido não encontrado.");
        }

        if (order.ExpirationDateTo.HasValue && order.ExpirationDateTo < DateTime.UtcNow)
        {
            _logger.LogWarning($"Pagamento recebido fora do prazo para Order {order.Id}, PaymentId: {paymentId}. Ignorado.");

            order.Status = OrderStatus.Expired;
            order.FailureReason = "O link de pagamento expirou. Por favor, gere um novo.";
            await _context.SaveChangesAsync();

            return PaymentWebhookResult.Fail("Pagamento fora do prazo.");
        }

        if (!isApproved)
        {
            order.Status = OrderStatus.Canceled;
            order.FailureReason = !string.IsNullOrWhiteSpace(payment.StatusDetail)
                ? StringExtensions.RemoveSpecialCharacters(payment.StatusDetail)
                : "Não conseguimos processar seu pagamento. Você pode tentar novamente em instantes ou até mesmo, utilizar outro método.";
            await _context.SaveChangesAsync();

            return PaymentWebhookResult.Fail(order.FailureReason);
        }

        order.Status = OrderStatus.Paid;
        await _context.SaveChangesAsync();

        return PaymentWebhookResult.Ok(order);
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, $"Erro ao processar webhook para PaymentId: {paymentId}");
        return PaymentWebhookResult.Fail("Erro interno ao processar o pagamento.");
    }
}


    public async Task ProcessMerchantOrderWebhookAsync(string merchantOrderId)
    {
        var merchantOrder = await _mercadoPagoClient.GetMerchantOrderByIdAsync(merchantOrderId);

        if (merchantOrder == null)
        {
            _logger.LogWarning($"Merchant Order {merchantOrderId} não encontrado.");
            return;
        }

        _logger.LogInformation($"Merchant Order recebido: {merchantOrder.Id}, Status: {merchantOrder.Status}");

        var externalRef = merchantOrder.ExternalReference;
        if (string.IsNullOrWhiteSpace(externalRef))
        {
            _logger.LogWarning($"Merchant Order {merchantOrderId} sem referência externa.");
            return;
        }

        var order = await _context.Orders.FirstOrDefaultAsync(o => o.ExternalPaymentId == externalRef);
        if (order == null)
        {
            _logger.LogWarning($"Pedido não encontrado para referência externa: {externalRef}");
            return;
        }

        if (order.Status == OrderStatus.Paid)
        {
            _logger.LogInformation($"Pedido {order.Id} já está pago. Nenhuma ação necessária.");
            return;
        }
    }
}
