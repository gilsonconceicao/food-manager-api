using MercadoPago.Resource.Preference;

namespace Domain.Interfaces; 

public interface IPaymentCommunication 
{
    Task<Preference> CreatePixAsync();
}