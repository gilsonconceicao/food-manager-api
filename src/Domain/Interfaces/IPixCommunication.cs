using MercadoPago.Client.Preference;
using MercadoPago.Resource.Preference;

namespace Domain.Interfaces;

public interface IPaymentCommunication
{
    Task<Preference> CreateCheckoutProAsync(
        List<PreferenceItemRequest> items
        // string payerName,
        // string payerSurname,
        // string payerEmail,
        // string notificationUrl,
        // string statementDescriptor
    );
}