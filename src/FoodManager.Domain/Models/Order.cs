
namespace FoodManager.Domain.Models
{
#nullable disable
    public class Order : BaseEntity
    {
        public int RequestNumber { get; set; }
        public User? User { get; set; }
        public Guid? UserId { get; set; }
        // public ICollection<Food> Foods { get; set; } = [];

        //relacionamento 
        public ICollection<FoodOrderRelation> FoodOrderRelations { get; set; } = new List<FoodOrderRelation>();
    }
}