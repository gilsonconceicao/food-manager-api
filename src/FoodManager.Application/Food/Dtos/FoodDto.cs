using FoodManager.Application.Orders.Dtos;
using FoodManager.Domain.Enums;
#nullable disable
namespace FoodManager.Application.Foods.Queries.GetAllWithPaginationFoodQuery
{
    public class FoodDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public decimal Price { get; set; }
        public string CategoryDisplay { get; set; }
        public string Category { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<OrdersRealatedFoodDto> Orders { get; set; }
    }
}