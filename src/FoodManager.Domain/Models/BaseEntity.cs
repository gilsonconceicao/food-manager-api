namespace FoodManager.Domain.Models;
public class BaseEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime CreatedAt { get; set; }
    public bool IsDeleted { get; set; }
}