
using Application.Users.Dtos;
using Domain.Enums;

namespace Application.Orders.Dtos;
#nullable disable
public class UpdateStepOrderDto
{
    public OrderStatus NewStatus { get; set; }
}