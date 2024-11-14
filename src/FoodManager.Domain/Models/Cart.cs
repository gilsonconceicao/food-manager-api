namespace FoodManager.Domain.Models; 

public class Cart : BaseEntity
{
    public Guid ItemId { get; set; }
    public int? Quantity { get; set; }
    public string? Resource { get; set; }
}