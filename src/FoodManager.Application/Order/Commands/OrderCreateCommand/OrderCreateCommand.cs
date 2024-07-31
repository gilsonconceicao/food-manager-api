using FoodManager.Application.FoodsOrders.Commands.Dtos;
using FoodManager.Domain.Enums;
using MediatR;

namespace FoodManager.Application.FoodsOrders.Commands.OrderCreateCommand;
#nullable disable
public class OrderCreateCommand : IRequest<bool>
{
    public ClientCreateDto Client { get; set; }
    public OrderStatus Status { get; set; }
}