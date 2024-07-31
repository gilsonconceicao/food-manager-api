using FoodManager.Domain.Enums;

namespace FoodManager.Domain.Models
{
    #nullable disable
    public class Order : BaseEntity 
    {
        public int RequestNumber { get; set; }
        public Client Client { get; set; }
        public ICollection<Food> Foods { get; set; }
    }
}