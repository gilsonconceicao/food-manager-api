using FoodManager.Application.Orders.Dtos;
using FoodManager.Domain.Enums;
#nullable disable
namespace FoodManager.Application.Foods.Queries.GetAllWithPaginationFoodQuery
{
    public class FoodDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string CategoryDisplay { get; set; }
        public FoodCategoryEnum? Category { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<OrderRelatedsDto> Orders { get; set; }
    }
}