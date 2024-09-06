
namespace FoodManager.Domain.Models
{
#nullable disable
    public class Order : BaseEntity
    {
        public int RequestNumber { get; set; }
        public ICollection<Food> Foods { get; set; } = [];
        public User? User { get; set; }
        public Guid? UserId { get; set; }
        public OrderFoodRelated? OrderFoodRelateds { get; } = null;

    }
}