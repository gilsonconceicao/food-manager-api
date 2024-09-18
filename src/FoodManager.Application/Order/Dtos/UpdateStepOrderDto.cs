
using FoodManager.Application.Users.Dtos;
using FoodManager.Domain.Enums;

namespace FoodManager.Application.Orders.Dtos;
#nullable disable
public class UpdateStepOrderDto
{
    public OrderStatus NewStatus { get; set; }
}