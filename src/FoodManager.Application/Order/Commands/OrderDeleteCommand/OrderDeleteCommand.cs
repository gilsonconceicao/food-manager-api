using MediatR;

namespace FoodManager.Application.Orders.Commands.OrderDeleteCommand; 
public class OrderDeleteCommand : IRequest<bool>
{
    public Guid OrderId { get; set; }
    public OrderDeleteCommand(Guid OrderId)
    {
        this.OrderId = OrderId;
    }
}