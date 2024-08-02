namespace FoodManager.Domain.Models; 

public class OrdersFoodsRelationship 
{
    public Guid FoodId { get; set; }
    public Guid OrderId { get; set; }

    public Food? Food { get; set; } = null;
    public Order? Order { get; set; } = null;
}