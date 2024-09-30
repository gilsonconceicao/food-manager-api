using FoodManager.Application.Foods.Queries.GetAllWithPaginationFoodQuery;

namespace FoodManager.Application.Orders.Dtos;
#nullable disable
public class OrderItemCreateDto
{
    public Guid FoodId { get; set; }
    public int Quantity { get; set; }
}