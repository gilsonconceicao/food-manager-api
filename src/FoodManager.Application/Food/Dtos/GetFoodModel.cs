using FoodManager.Domain.Enums;
#nullable disable
namespace FoodManager.Application.Foods.Queries.GetAllWithPaginationFoodQuery
{
    public class GetFoodModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string CategoryDisplay { get; set; }
        public decimal Price { get; set; }
        public FoodCategory? Category { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}