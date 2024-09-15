using AutoMapper;
using FoodManager.Domain.Models;
using FoodManager.Application.Orders.Dtos;

namespace FoodManager.Application.Mappings;

public class OrderItemsMapper : Profile
{
    public OrderItemsMapper()
    {
        CreateMap<OrderItems, OrderItemsDto>()
            .ReverseMap();
    }
}
