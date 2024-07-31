using FoodManager.Application.Orders.Commands.Dtos;
using MediatR;

namespace FoodManager.Application.Orders.Commands.OrderCreateCommand;
#nullable disable
public class OrderCreateCommand : IRequest<bool>
{
    public ClientCreateDto Client { get; set; }
    public List<Guid> FoodsIds { get; set; }
}