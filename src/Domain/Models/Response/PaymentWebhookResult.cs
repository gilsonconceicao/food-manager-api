using Domain.Models;

public class PaymentWebhookResult
{
    public bool Success { get; set; }
    public Order? Order { get; set; }
    public string? Message { get; set; }

    public static PaymentWebhookResult Ok(Order order) =>
        new PaymentWebhookResult { Success = true, Order = order };

    public static PaymentWebhookResult Fail(string message) =>
        new PaymentWebhookResult { Success = false, Message = message };
}
