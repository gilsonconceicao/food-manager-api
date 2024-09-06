namespace FoodManager.Domain.Models;
#nullable disable
public class FoodOrderRelation
{
    public int OrderId { get; set; }
    public Order Order { get; set; }

    public int FoodId { get; set; }
    public Food Food { get; set; }
}