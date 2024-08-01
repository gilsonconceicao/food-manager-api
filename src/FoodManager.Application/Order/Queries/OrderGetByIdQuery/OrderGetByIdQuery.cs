using FoodManager.Application.Orders.Dtos;
using FoodManager.Domain.Models;
using MediatR;

namespace FoodManager.Application.Orders.Queries.OrderGetByIdQuery; 

public class OrderGetByIdQuery : IRequest<Order>
{
    public Guid OrderId { get; set;}
    public OrderGetByIdQuery(Guid OrderId)
    {
        this.OrderId = OrderId;
    }
}