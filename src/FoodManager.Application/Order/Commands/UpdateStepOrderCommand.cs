using FoodManager.Domain.Enums;
using MediatR;

namespace FoodManager.Application.Orders.Commands; 

public class UpdateStepOrderCommand : IRequest<OrderStatus> 
{
    public Guid Id { get; set; }
    public OrderStatus NewStatus { get; set; }

}

public class UpdateStepOrderHandler : IRequestHandler<UpdateStepOrderCommand, OrderStatus>
{
    public Task<OrderStatus> Handle(UpdateStepOrderCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();


    }
}