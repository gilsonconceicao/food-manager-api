namespace FoodManager.Domain.Models;

public class OrderFoodRelated
{
    public Guid OrderId { get; set; }
    public Guid FoodId { get; set;}
    public ICollection<Order>? Order { get; set; } = null; 
    public ICollection<Food>? Food { get; set; } = null;
}