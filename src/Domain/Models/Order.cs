using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using Domain.Enums;

#nullable enable

namespace Domain.Models
{
    public class Order : BaseEntity
    {
        public Pay Pay { get; set; }
        public string? PaymentId { get; set; }

        public string? ExternalReference { get; set; }

        public string? FailureReason { get; set; }
        
        public string? Observations { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal? TotalValue { get; set; }

        public int RequestNumber { get; set; }

        public OrderStatus Status { get; set; } = OrderStatus.AwaitingPayment;

        public User? User { get; set; }

        public Guid? UserId { get; set; }

        public ICollection<OrderItems> Items { get; set; } = new List<OrderItems>();
    }
}