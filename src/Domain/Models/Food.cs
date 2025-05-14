using System.ComponentModel.DataAnnotations.Schema;
using Domain.Enums;
#nullable disable
namespace Domain.Models
{
    public class Food : BaseEntity
    {
        private string _urlImage;
        public string UrlImage
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_urlImage))
                    return "https://default-image.com/placeholder.png";

                return $"https://{_urlImage}";
            }
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    _urlImage = value.Replace("https://", "").Trim();
                }
                else
                {
                    _urlImage = null;
                }
            }
        }

        [Column(TypeName = "varchar(50)")]
        public string Name { get; set; }
                
        public string Description { get; set; }

        public bool IsAvailable { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }
        
        public FoodCategoryEnum? Category { get; set; }
        
        public ICollection<OrderItems> Items { get; set; } = new List<OrderItems>();
    }
}