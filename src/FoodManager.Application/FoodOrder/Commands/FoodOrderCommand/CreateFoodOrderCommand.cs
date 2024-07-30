using FoodManager.Application.FoodOrders.Commands.Dtos;
using FoodManager.Domain.Enums;
using MediatR;

namespace FoodManager.Application.FoodOrders.Commands.CreateFoodOrderCommand;
#nullable disable
public class CreateFoodOrderCommand : IRequest<bool>
{
    public ClientCreateDto Client { get; set; }
    public StatusFoodOrder Status { get; set; }
}