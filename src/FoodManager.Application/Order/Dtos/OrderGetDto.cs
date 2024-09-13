using FoodManager.Application.Foods.Queries.GetAllWithPaginationFoodQuery;
using FoodManager.Application.Users.Dtos;

namespace FoodManager.Application.Orders.Dtos;
#nullable disable
public class OrderGetDto
{
    public Guid Id { get; set; }
    public Guid? UserId { get; set; }
    public int OrderNumber { get; set; }
    public DateTime CreatedAt { get; set; }
    public UserRelatedOrderDto User { get; set; }
    public List<FoodItemsDto> Foods { get; set; }
}