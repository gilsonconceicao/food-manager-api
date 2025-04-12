
using Domain.Enums;

namespace Domain.Models
{
#nullable disable
    public class Order : BaseEntity
    {
        public string? PaymentId { get; set; }

        public int RequestNumber { get; set; }

        public OrderStatus Status { get; set; } = OrderStatus.AwaitingPayment;

        public User? User { get; set; }
        public Guid? UserId { get; set; }

        public ICollection<OrderItems> Items { get; set; } = new List<OrderItems>();
    }
}