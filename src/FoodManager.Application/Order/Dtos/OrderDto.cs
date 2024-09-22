
using FoodManager.Application.Users.Dtos;
using FoodManager.Domain.Enums;

namespace FoodManager.Application.Orders.Dtos;
#nullable disable
public class OrderDto
{
    public Guid Id { get; set; }
    public int OrderNumber { get; set; }
    public OrderStatus Status { get; set; }
    public string StatusDisplay { get; set; }
    public DateTime CreatedAt { get; set; }
    public CreatedByDto CreatedBy { get; set; }
    public List<OrderItemsDto> Items { get; set; }
}