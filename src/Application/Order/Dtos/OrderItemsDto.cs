using Application.Foods.Queries.GetAllWithPaginationFoodQuery;

namespace Application.Orders.Dtos; 
#nullable disable
public class OrderItemsDto 
{
    public Guid? FoodId { get; set; }
    public decimal Price { get; set; }
    public decimal Discount { get; set; }
    public int Quantity { get; set; }
    public DateTime CreatedAt { get; set; }
    public FoodItemsDto? Food { get; set; }
}