
using FoodManager.Domain.Enums;

namespace FoodManager.Domain.Models
{
#nullable disable
    public class Order : BaseEntity
    {
        public int RequestNumber { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Created;
        public User? User { get; set; }
        public Guid? UserId { get; set; }
        public ICollection<OrderItems> Items { get; set; } = new List<OrderItems>();
    }
}