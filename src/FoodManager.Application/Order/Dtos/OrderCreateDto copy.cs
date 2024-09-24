
using FoodManager.Domain.Enums.Triggers;

namespace FoodManager.Application.Orders.Dtos
{
    #nullable disable
    public class ProcessStepDto 
    {
        public OrderTrigger OrderTrigger { get; set; }
    }
}