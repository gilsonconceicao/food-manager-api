using FoodManager.Application.FoodsOrders.Commands.Dtos;
using MediatR;

namespace FoodManager.Application.FoodsOrders.Commands.OrderCreateCommand;
#nullable disable
public class OrderCreateCommand : IRequest<bool>
{
    public ClientCreateDto Client { get; set; }
    public List<Guid> FoodsIds { get; set; }
}