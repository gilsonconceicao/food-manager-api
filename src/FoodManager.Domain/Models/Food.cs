using FoodManager.Domain.Enums;
#nullable disable
namespace FoodManager.Domain.Models
{
    public class Food : BaseEntity
    {
        public Guid? OrderId { get; set; }
        public Order Order { get; set; }
        public string Name { get; set; }
        public string UrlImage { get; set; }
        public string Description { get; set; }
        public bool IsAvailable { get; set; }
        public decimal Price { get; set; }
        public FoodCategory? Category { get; set; }
        public string PreparationTime { get; set; } 
    }
}