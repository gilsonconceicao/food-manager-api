using System.ComponentModel.DataAnnotations;

#nullable disable

namespace FoodManager.Domain.Models;
public class BaseEntity
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public bool IsDeleted { get; set; }
    public Guid CreatedByUserId { get; set; }
    public string CreatedByUserName { get; set; }
    public Guid? UpdatedByUserId { get; set; }
    public string UpdatedByUserName { get; set; }
}