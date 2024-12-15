using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace Domain.Models;
public class BaseEntity
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; } = null;

    public bool IsDeleted { get; set; } = false;

    [Column(TypeName = "varchar(35)")]
    public string? CreatedByUserId { get; set; } = null;

    public string? CreatedByUserName { get; set; } = null;

    [Column(TypeName = "varchar(35)")]
    public string? UpdatedByUserId { get; set; } = null;

    public string? UpdatedByUserName { get; set; } = null;
}