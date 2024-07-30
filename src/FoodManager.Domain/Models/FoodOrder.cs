using FoodManager.Domain.Enums;

namespace FoodManager.Domain.Models
{
    #nullable disable
    public class FoodOrder : BaseEntity 
    {
        public int RequestNumber { get; set; }
        public Client Client { get; set; }
        public StatusFoodOrder Status { get; set; }
        public ICollection<Food> Foods { get; set; }
    }
}