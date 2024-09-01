using FoodManager.Application.Orders.Commands.Dtos;
using MediatR;

namespace FoodManager.Application.Orders.Commands.OrderCreateCommand;
#nullable disable
public class OrderCreateCommand : IRequest<bool>
{
    public List<Guid> FoodsIds { get; set; }
}