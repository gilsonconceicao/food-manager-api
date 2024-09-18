using FoodManager.Domain.Enums;
#nullable disable
namespace FoodManager.Domain.Models
{
    public class Food : BaseEntity
    {
        public string Name { get; set; }
        public string UrlImage { get; set; }
        public string Description { get; set; }
        public bool IsAvailable { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public FoodCategoryEnum? Category { get; set; }
        public ICollection<OrderItems> Items { get; set; } = new List<OrderItems>();
    }
}