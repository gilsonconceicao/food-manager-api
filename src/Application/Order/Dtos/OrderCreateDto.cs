namespace Application.Orders.Dtos
{
#nullable disable
    public class OrderCreateDto
    {
        public List<OrderItemCreateDto> Foods { get; set; }
    }
}