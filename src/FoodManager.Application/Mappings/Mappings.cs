using AutoMapper;
using FoodManager.Application.Orders.Commands.OrderCreateCommand;
using FoodManager.Application.Orders.Commands.Dtos;
using FoodManager.Application.Foods.Commands.FoodCreateCommand;
using FoodManager.Application.Foods.Queries.GetAllWithPaginationFoodQuery;
using FoodManager.Domain.Extensions;
using FoodManager.Domain.Models;
using FoodManager.Application.Foods.Commands.Dtos;
using FoodManager.Application.Orders.Dtos;
using FoodManager.Application.orders.Dtos;

#nullable disable
namespace FoodManager.Application.Mappings;
public class Mappings : Profile
{
    public Mappings()
    {
        CreateMap<FoodCreateCommand, Food>();
        CreateMap<FoodCreateDto, Food>();

        CreateMap<OrderCreateCommand, Order>();
        CreateMap<Order, OrderGetDto>()
            .ForMember(x => x.OrderNumber, src => src.MapFrom(x => x.RequestNumber));

        CreateMap<AddressCreateDto, Address>();

        CreateMap<Food, GetFoodModel>()
            .ForMember(x => x.CategoryDisplay,
                src => src.MapFrom(x => x.Category.GetDescription()))
            .ForMember(x => x.OrderId,
                src => src.MapFrom(x => x.OrderId == Guid.Empty ? (Guid?)null : x.OrderId))
            .ReverseMap();
    }
}
