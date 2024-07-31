using AutoMapper;
using crud_products_api.src.Models;
using FoodManager.Application.FoodsOrders.Commands.OrderCreateCommand;
using FoodManager.Application.FoodsOrders.Commands.Dtos;
using FoodManager.Application.Foods.Commands.FoodCreateCommand;
using FoodManager.Application.Foods.Queries.GetAllWithPaginationFoodQuery;
using FoodManager.Domain.Extensions;
using FoodManager.Domain.Models;
using FoodManager.Application.Foods.Commands.Dtos;

#nullable disable
namespace FoodManager.Application.Mappings;
public class Mappings : Profile
{
    public Mappings()
    {
        CreateMap<FoodCreateCommand, Food>();
        CreateMap<FoodCreateDto, Food>();

        CreateMap<OrderCreateCommand, Order>(); 
        CreateMap<ClientCreateDto, Client>();
        CreateMap<AddressCreateDto, Address>();

        CreateMap<Food, GetFoodModel>()
            .ForMember(x => x.CategoryDisplay,  
                src => src.MapFrom(x => x.Category.GetDescription()))
            .ReverseMap();
    }
}
