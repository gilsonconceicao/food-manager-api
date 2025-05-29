using Domain.Models;
using MercadoPago.Client.Common;
using MercadoPago.Client.Payment;

namespace Integrations.MercadoPago.Factories;
#nullable disable

public interface IPaymentFactory
{
    PaymentCreateRequest CreatePayment(
        PaymentMethodEnum paymentMethod,
        User user,
        decimal amount,
        string description,
        string notificationUrl,
        string token = null,
        int installments = 1, 
        string paymentMethodId = null 
    );
}

public class PaymentFactory : IPaymentFactory
{
    public PaymentCreateRequest CreatePayment(
        PaymentMethodEnum paymentMethod,
        User user,
        decimal amount,
        string description,
        string notificationUrl,
        string token = null,
        int installments = 1, 
        string paymentMethodId = null
    )
    {
        var payer = new PaymentPayerRequest
        {
            Email = user.Email,
            FirstName = user.Name,
            LastName = "" 
        };

        return paymentMethod switch
        {
            PaymentMethodEnum.Pix => CreatePixPayment(payer, amount, description, notificationUrl),

            PaymentMethodEnum.Card => CreateCardPayment(payer, amount, description, notificationUrl, token, installments, paymentMethodId),

            _ => throw new NotImplementedException($"Método de pagamento '{paymentMethod}' não implementado.")
        };
    }

    private PaymentCreateRequest CreatePixPayment(
        PaymentPayerRequest payer,
        decimal amount,
        string description,
        string notificationUrl
    ) => new()
    {
        TransactionAmount = amount,
        Description = description,
        PaymentMethodId = "pix",
        Payer = payer,
        NotificationUrl = notificationUrl,
        ExternalReference = Guid.NewGuid().ToString(), 
        DateOfExpiration = DateTime.UtcNow.AddHours(1)
    };

    private PaymentCreateRequest CreateCardPayment(
        PaymentPayerRequest payer,
        decimal amount,
        string description,
        string notificationUrl,
        string token,
        int installments, 
        string paymentMethodId
    )
    {
        if (string.IsNullOrEmpty(token))
            throw new ArgumentException("Token do cartão é obrigatório para pagamentos com cartão.");

        return new PaymentCreateRequest
        {
            TransactionAmount = amount,
            Description = description,
            PaymentMethodId = paymentMethodId,
            Token = token,
            Installments = installments,
            Payer = payer,
            NotificationUrl = notificationUrl,
            ExternalReference = Guid.NewGuid().ToString(),
            DateOfExpiration = DateTime.UtcNow.AddHours(1),

        }; 
    }
}
