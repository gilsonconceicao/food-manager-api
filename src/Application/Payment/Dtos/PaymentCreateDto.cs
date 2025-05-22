namespace Application.Payment.Dtos;
#nullable disable
public class PaymentCreateDto
{
    public PaymentMethodEnum PaymentMethod { get; set; }
    public string Description { get; set; }
    public decimal Amount { get; set; }
    public string NotificationUrl { get; set; }
    public int Installments { get; set; } = 1;
    public string Token { get; set; }
}