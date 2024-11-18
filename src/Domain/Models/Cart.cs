namespace Domain.Models; 
#nullable disable
public class Cart : BaseEntity
{
    public Guid ItemId { get; set; }
    public int? Quantity { get; set; }
}