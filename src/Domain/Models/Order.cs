using System.ComponentModel.DataAnnotations.Schema;
using Domain.Enums;

#nullable enable

namespace Domain.Models
{
    public class Order : BaseEntity
    {
        public string? ExternalPaymentId { get; set; }

        public string? FailureReason { get; set; }

        public int? NumberOfInstallments { get; set; } = 1;

        [Column(TypeName = "decimal(18, 2)")]
        public decimal? TotalValue { get; set; }

        public DateTime? ExpirationDateTo { get; set; }

        public int RequestNumber { get; set; }

        public OrderStatus Status { get; set; } = OrderStatus.AwaitingPayment;

        public User? User { get; set; }

        public Guid? UserId { get; set; }

        public ICollection<OrderItems> Items { get; set; } = new List<OrderItems>();
    }
}