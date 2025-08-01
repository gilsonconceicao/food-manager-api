using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models;
#nullable disable
public class OrderItems
{
    [Key]
    public Guid OrderId { get; set; }
    
    public Order Order { get; set; }
    
    [Key]
    public Guid FoodId { get; set; }
    
    public Food Food { get; set; }
    
    public User? User { get; set; }
    
    public Guid? UserId { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? Price { get; set; }
    
    public int? Quantity { get; set; }
    
    [Column(TypeName = "decimal(18, 2)")]
    public decimal? Discount { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}