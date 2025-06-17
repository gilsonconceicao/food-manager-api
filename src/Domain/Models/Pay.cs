using System.ComponentModel.DataAnnotations;

namespace Domain.Models;
#nullable disable
public class Pay
{
    [Key]
    public string Id { get; set; }
    public Guid OrderId { get; set; }
    public Order Order { get; set; }
    public string Description { get; set; }
    public string Status { get; set; }
    public string PaymentTypeId { get; set; }
    public string PaymentMethodId { get; set; }
    public string CurrencyId { get; set; }
    public int? Installments { get; set; }
    public decimal? TransactionAmount { get; set; }
    public string ExternalReference { get; set; }
    public string NotificationUrl { get; set; }
    public DateTime DateCreated { get; set; }
    public DateTime DateLastUpdated { get; set; }
    public DateTime ExpirationDateTo { get; set; }
    public string QrCode { get; set; }
    public string QrCodeBase64 { get; set; }

    public long CollectorId { get; set; }
    public string IssuerId { get; set; }

}
