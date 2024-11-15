using System.ComponentModel.DataAnnotations;

#nullable disable

namespace FoodManager.Domain.Models;
public class BaseEntity
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsDeleted { get; set; } = false;
    public DateTime? UpdatedAt { get; set; } = null;
    public string? CreatedByUserId { get; set; } = null;
    public string? CreatedByUserName { get; set; } = null;
    public string? UpdatedByUserId { get; set; } = null;
    public string? UpdatedByUserName { get; set; } = null;
}