using Domain.Models.Request;
using MercadoPago.Client.Payment;
namespace Integrations.Interfaces;

public interface IPaymentCommunication
{
    Task<PaymentCreateRequest> CreatePaymentAsync(
        PaymentMethodEnum paymentMethod,
        decimal amount,
        string description,
        int installments = 1,
        CardDataRequestDto? Card = null
    );
    Task<PaymentWebhookResult> ProcessPaymentWebhookAsync(string paymentId, CancellationToken cancellationToken = default);
}