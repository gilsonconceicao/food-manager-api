
using Application.Users.Dtos;
using Domain.Enums;

namespace Application.Orders.Dtos;
#nullable disable
public class OrderDto
{
    public Guid Id { get; set; }
    public string ExternalPaymentId { get; set; }
    public string PaymentId { get; set; }
    public int OrderNumber { get; set; }
    public OrderStatus Status { get; set; }
    public string StatusDisplay { get; set; }
    public DateTime CreatedAt { get; set; }
    public CreatedByDto CreatedBy { get; set; }
    public List<OrderItemsDto> Items { get; set; }
}