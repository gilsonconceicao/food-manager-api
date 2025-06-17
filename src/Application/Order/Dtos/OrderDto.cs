
using Application.Users.Dtos;
using Domain.Enums;

namespace Application.Orders.Dtos;
#nullable disable
public class OrderDto
{
    public Guid Id { get; set; }
    public string PaymentId { get; set; }
    public int OrderNumber { get; set; }
    public string Status { get; set; }
    public string FailureReason { get; set; }
    public string StatusDisplay { get; set; }
    public string Observations { get; set; }
    public int NumberOfInstallments { get; set; }
    public decimal TotalValue { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public CreatedByDto CreatedBy { get; set; }
    public List<OrderItemsDto> Items { get; set; }
}