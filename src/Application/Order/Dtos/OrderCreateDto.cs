namespace Application.Orders.Dtos
{
#nullable disable
    public class OrderCreateDto
    {
        public List<Guid> CartIds { get; set; }
    }
}