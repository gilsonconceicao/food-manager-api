using Api.Services;
using Domain.Enums;
using Domain.Extensions;
using Domain.Helpers;
using Domain.Models.Request;
using Infrastructure.Database;
using Integrations.Interfaces;
using Integrations.MercadoPago.Factories;
using Integrations.Settings;
using MercadoPago.Client.Payment;
using MercadoPago.Config;
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
    private readonly IPaymentFactory _paymentFactory;

    public PaymentCommunication(
        IOptions<MercadoPagoSettings> mercadoPagoSettings,
        ICurrentUser httpUserService,
        DataBaseContext context,
        IMercadoPagoClient mercadoPagoClient,
        ILogger<PaymentCommunication> logger,
        IPaymentFactory paymentFactory
    )
    {
        _mercadoPagoSettings = mercadoPagoSettings.Value;
        _httpUserService = httpUserService;
        _context = context;
        _mercadoPagoClient = mercadoPagoClient;
        _logger = logger;
        _paymentFactory = paymentFactory;
    }

    public async Task<PaymentCreateRequest> CreatePaymentAsync(
        PaymentMethodEnum paymentMethod,
        decimal amount,
        string description,
        int installments = 1,
        CardDataRequestDto? card = null
    )
    {
        MercadoPagoConfig.AccessToken = _mercadoPagoSettings.AccessToken;

        var userAuthenticated = await _httpUserService.GetAuthenticatedUser();

        var user = _context
            .Users
            .FirstOrDefault(x => x.FirebaseUserId == userAuthenticated.UserId)
            ?? throw new Exception("Usuário autenticado não encontrado na base.");

        var cardTokenData = await _mercadoPagoClient.CreateCardTokenAsync(card) ?? null;

        var paymentRequest = _paymentFactory.CreatePayment(
            paymentMethod,
            user,
            amount,
            description,
            _mercadoPagoSettings.NotificationUrl,
            cardTokenData.Token,
            installments
        );

        return paymentRequest;
    }

    public async Task<PaymentWebhookResult> ProcessPaymentWebhookAsync(string paymentId, CancellationToken cancellationToken = default)
    {
        try
        {
            var payment = await _mercadoPagoClient.GetPaymentByIdAsync(paymentId)
                                                   .ConfigureAwait(false);

            _logger.LogInformation($"Processando webhook para PaymentId: {paymentId}");

            if (payment == null)
            {
                _logger.LogWarning($"Pagamento não encontrado no mercado pago para ID: {paymentId}");
                return PaymentWebhookResult.Fail("Pagamento não encontrado.");
            }

            var paymentClientId = payment.PaymentId.ToString();
            bool isApproved = payment.Status == "approved";

            _logger.LogInformation($"Pagamento localizado: PreferenceId {paymentClientId}, Status {payment.Status}");

            var order = await Helpers.RetryAsync(
                () => _context.Orders
                    .Include(o => o.Pay)
                    .FirstOrDefaultAsync(o => o.PaymentId == paymentClientId, cancellationToken),
                3,
                1500
            );

            if (order == null)
            {
                _logger.LogWarning($"Pedido não encontrado pelo paymentId: {paymentClientId}");
                return PaymentWebhookResult.Fail("Pedido não encontrado.");
            }

            if (order.Pay.ExpirationDateTo != null && order.Pay.ExpirationDateTo < DateTime.UtcNow)
            {
                _logger.LogWarning($"Pagamento recebido fora do prazo para Order {order.Id}, PaymentId: {paymentId}. Ignorado.");

                order.Status = OrderStatus.Expired;
                order.FailureReason = "O link de pagamento expirou. Por favor, gere um novo.";
                await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

                return PaymentWebhookResult.Fail("Pagamento fora do prazo.");
            }

            if (!isApproved)
            {
                order.Status = OrderStatus.Cancelled;
                order.FailureReason = !string.IsNullOrWhiteSpace(payment.StatusDetail)
                    ? StringExtensions.RemoveSpecialCharacters(payment.StatusDetail)
                    : "Não conseguimos processar seu pagamento. Você pode tentar novamente em instantes ou até mesmo, utilizar outro método.";

                await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
                _logger.LogInformation($"Pagamento não aprovado para Order {order.Id}. Status definido como Cancelled.");

                return PaymentWebhookResult.Fail(order.FailureReason);
            }

            if (order.Status != OrderStatus.Paid)
            {
                order.Status = OrderStatus.Paid;
                order.FailureReason = null;
                await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
                _logger.LogInformation($"Pedido {order.Id} atualizado para status Paid.");
            }
            else
            {
                _logger.LogInformation($"Pedido {order.Id} já está com status Paid.");
            }

            return PaymentWebhookResult.Ok(order);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Erro ao processar webhook para PaymentId: {paymentId}");
            return PaymentWebhookResult.Fail("Erro interno ao processar o pagamento.");
        }
    }
}
