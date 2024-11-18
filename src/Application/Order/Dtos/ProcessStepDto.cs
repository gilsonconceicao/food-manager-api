
using Domain.Enums.Triggers;

namespace Application.Orders.Dtos
{
    #nullable disable
    public class ProcessStepDto 
    {
        public OrderTrigger OrderTrigger { get; set; }
    }
}