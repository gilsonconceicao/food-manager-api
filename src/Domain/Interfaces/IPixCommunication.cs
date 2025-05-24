using MercadoPago.Client.Payment;
namespace Domain.Interfaces;

public interface IPaymentCommunication
{
    Task<PaymentCreateRequest> CreatePaymentAsync(PaymentMethodEnum paymentMethod, decimal amount, string description, int installments = 1, string? token = null);
    Task<PaymentWebhookResult> ProcessPaymentWebhookAsync(string paymentId, CancellationToken cancellationToken = default);
}