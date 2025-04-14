using Domain.Models;
using MercadoPago.Client.Preference;
using MercadoPago.Resource.Preference;

namespace Domain.Interfaces;

public interface IPaymentCommunication
{
    Task<Preference> CreateCheckoutProAsync(List<PreferenceItemRequest> items);
    Task<PaymentWebhookResult> ProcessPaymentWebhookAsync(string paymentId);
    Task ProcessMerchantOrderWebhookAsync(string merchantOrderId);
}