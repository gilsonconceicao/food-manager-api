namespace Domain.Models; 
#nullable enable
public class Cart : BaseEntity
{
    public Guid FoodId { get; set; }
    public virtual Food Food {get; set; } 
    public int? Quantity { get; set; }
    public string? Observations { get; set; }
}