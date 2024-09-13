using AutoMapper;
using FoodManager.Domain.Models;
using FoodManager.Application.Orders.Dtos;
using FoodManager.Application.Orders.Commands;

namespace FoodManager.Application.Mappings;

public class OrderMappers : Profile
{
    public OrderMappers()
    {
        CreateMap<OrderCreateCommand, Order>();
        CreateMap<Order, OrderGetDto>()
            .ForMember(x => x.OrderNumber, src => src.MapFrom(x => x.RequestNumber));
            // .ForMember(x => x.Foods, src => src.MapFrom(x => x.FoodOrderRelations.Select(x => x.Food)));

        CreateMap<Order, OrderRelatedsDto>()
            .ForMember(x => x.OrderNumber, src => src.MapFrom(x => x.RequestNumber));
            
        CreateMap<Order, OrderListDto>()
            .ForMember(x => x.OrderNumber, src => src.MapFrom(x => x.RequestNumber));
            // .ForMember(x => x.Foods, src => src.MapFrom(x => x.FoodOrderRelations.Select(x => x.Food)));
    }
}
