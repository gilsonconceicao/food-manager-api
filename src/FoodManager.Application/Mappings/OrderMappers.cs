using AutoMapper;
using FoodManager.Domain.Models;
using FoodManager.Application.Orders.Dtos;
using FoodManager.Application.Orders.Commands;
using FoodManager.Application.Utils;

namespace FoodManager.Application.Mappings;

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
