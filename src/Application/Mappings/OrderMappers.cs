using AutoMapper;
using Domain.Models;
using Application.Orders.Dtos;
using Application.Orders.Commands;
using Application.Utils;

namespace Application.Mappings;

public class OrderMappers : Profile
{
    public OrderMappers()
    {
        CreateMap<OrderCreateCommand, Order>();

        CreateMap<Order, OrdersRealatedFoodDto>()
            .ForMember(x => x.OrderNumber, src => src.MapFrom(x => x.RequestNumber));
            
        CreateMap<Order, OrderDto>()
            .ForMember(x => x.OrderNumber, src => src.MapFrom(x => x.RequestNumber))
            .ForMember(x => x.CreatedBy, src => src.MapFrom(x => x.User))
            .ForMember(x => x.StatusDisplay, src => src.MapFrom(x => x.Status.GetDescription()));
    }
}
