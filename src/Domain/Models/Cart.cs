namespace Domain.Models; 
#nullable disable
public class Cart : BaseEntity
{
    public Guid FoodId { get; set; }
    public virtual Food Food {get; set; } 
    public int? Quantity { get; set; }
}