namespace FoodManager.Domain.Models;
#nullable disable
public class FoodOrderRelation
{
    public Guid OrderId { get; set; }
    public Order Order { get; set; }

    public Guid FoodId { get; set; }
    public Food Food { get; set; }
}