namespace FoodManager.Domain.Models; 
#nullable disable
public class Cart : BaseEntity
{
    public string UserId { get; set; }
    public Guid ItemId { get; set; }
    public int? Quantity { get; set; }
}