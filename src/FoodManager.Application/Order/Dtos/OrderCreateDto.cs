namespace FoodManager.Application.Orders.Dtos
{
    #nullable disable
    public class OrderCreateDto 
    {
        public List<Guid> FoodsIds { get; set; }
    }
}