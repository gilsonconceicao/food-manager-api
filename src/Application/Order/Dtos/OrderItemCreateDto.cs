using Application.Foods.Queries.GetAllWithPaginationFoodQuery;

namespace Application.Orders.Dtos;
#nullable disable
public class OrderItemCreateDto
{
    public Guid FoodId { get; set; }
    public int? Quantity { get; set; }
    public string? Observations { get; set; }
}