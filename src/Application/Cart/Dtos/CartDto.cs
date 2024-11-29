using Application.Foods.Queries.GetAllWithPaginationFoodQuery;

namespace Application.Carts.Dtos; 
public class CartDto
{
    public Guid Id { get; set; }
    public FoodDto Food { get; set; }
    public Guid ItemId { get; set; }
    public int? Quantity { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? CreatedByUserId { get; set; } = null;
    public string? CreatedByUserName { get; set; } = null;
}