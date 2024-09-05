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
        public FoodCategoryEnum? Category { get; set; }
        public string PreparationTime { get; set; }
        public ICollection<Order> Orders { get; set; }
    }
}