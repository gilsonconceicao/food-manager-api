using AutoMapper;
using crud_products_api.src.Models;
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
        CreateMap<Order, OrderGetDto>(); 
        
        CreateMap<ClientCreateDto, Client>();
        CreateMap<AddressCreateDto, Address>();
        
        CreateMap<ClientGetDto, Client>()
            .ReverseMap();

        CreateMap<Food, GetFoodModel>()
            .ForMember(x => x.CategoryDisplay,  
                src => src.MapFrom(x => x.Category.GetDescription()))
            .ReverseMap();
    }
}
