namespace Application.Orders.Dtos;
#nullable disable
public class OrderDto : BaseModelResponse
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
    public List<OrderItemsDto> Items { get; set; }
}