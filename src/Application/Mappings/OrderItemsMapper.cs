using AutoMapper;
using Domain.Models;
using Application.Orders.Dtos;

namespace Application.Mappings;

public class OrderItemsMapper : Profile
{
    public OrderItemsMapper()
    {
        CreateMap<OrderItems, OrderItemsDto>()
            .ReverseMap();
    }
}
