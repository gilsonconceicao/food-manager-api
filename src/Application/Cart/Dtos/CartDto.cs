using Application.Foods.Queries.GetAllWithPaginationFoodQuery;

namespace Application.Carts.Dtos; 
public class CartDto : BaseModelResponse
{
    public Guid Id { get; set; }
    public FoodDto Food { get; set; }
    public Guid FoodId { get; set; }
    public int? Quantity { get; set; }
    public string? Observations {get; set;}
}