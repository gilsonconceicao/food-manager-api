using FoodManager.Application.Foods.Queries.GetAllWithPaginationFoodQuery;

namespace FoodManager.Application.Orders.Dtos;
#nullable disable
public class OrderGetDto
{
    public Guid Id { get; set; }
    public int OrderNumber { get; set; }
    public List<GetFoodDto> Foods { get; set; }
    public Guid? UserId { get; set; }
}