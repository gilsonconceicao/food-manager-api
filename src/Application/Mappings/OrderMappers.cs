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
            .ForMember(o => o.OrderNumber, src => src.MapFrom(o => o.RequestNumber));

        CreateMap<Order, OrderDto>()
            .ForMember(o => o.OrderNumber, src => src.MapFrom(o => o.RequestNumber))
            .ForMember(o => o.CreatedBy, src => src.MapFrom(o => o.User))
            .ForMember(o => o.StatusDisplay, src => src.MapFrom(o => o.Status.GetDescription()))
            .ForMember(o => o.Status, src => src.MapFrom(o => o.Status.ToString()))
            .ForMember(o => o.TotalValue, src => src.MapFrom(o => o.Items.Select(i => i.Price * i.Quantity).Sum()));
    }
}
