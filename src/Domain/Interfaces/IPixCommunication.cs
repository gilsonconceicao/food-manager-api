using Domain.Models;
using MercadoPago.Client.Preference;
using MercadoPago.Resource.Payment;
using MercadoPago.Resource.Preference;

namespace Domain.Interfaces;

public interface IPaymentCommunication
{
    Task<Payment> CreatePaymentAsync(PaymentMethodEnum paymentMethod, decimal amount, string description, int installments = 1, string? token = null);
    Task<PaymentWebhookResult> ProcessPaymentWebhookAsync(string paymentId, CancellationToken cancellationToken = default);
    Task ProcessMerchantOrderWebhookAsync(string merchantOrderId);
}