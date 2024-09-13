
using FoodManager.Application.Users.Dtos;

namespace FoodManager.Application.Orders.Dtos;
#nullable disable
public class OrderListDto
{
    public Guid Id { get; set; }
    public int OrderNumber { get; set; }
    public DateTime CreatedAt { get; set; }
    public CreatedByDto CreatedBy { get; set; }
    public List<OrderItemsDto> Items { get; set; }

}