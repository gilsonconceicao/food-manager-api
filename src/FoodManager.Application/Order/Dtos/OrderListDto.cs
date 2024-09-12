using FoodManager.Application.Foods.Queries.GetAllWithPaginationFoodQuery;

namespace FoodManager.Application.Orders.Dtos;
#nullable disable
public class OrderListDto
{
    public Guid Id { get; set; }
    public Guid? UserId { get; set; }
    public int OrderNumber { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<FoodsRelatedsDto> Foods { get; set; }

}