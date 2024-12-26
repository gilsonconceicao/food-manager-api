using System.ComponentModel.DataAnnotations.Schema;
using Domain.Enums;
#nullable disable
namespace Domain.Models
{
    public class Food : BaseEntity
    {
        [Column(TypeName = "varchar(50)")]
        public string Name { get; set; }
        
        public string UrlImage { get; set; }
        
        public string Description { get; set; }

        public bool IsAvailable { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }
        
        public FoodCategoryEnum? Category { get; set; }
        
        public ICollection<OrderItems> Items { get; set; } = new List<OrderItems>();
    }
}