using AutoMapper;
using FoodManager.Application.Foods.Commands;
using FoodManager.Application.Foods.Queries.GetAllWithPaginationFoodQuery;
using FoodManager.Domain.Extensions;
using FoodManager.Domain.Models;
using FoodManager.Application.Foods.Commands.Dtos;

namespace FoodManager.Application.Mappings;

public class FoodMappers : Profile
{
    public FoodMappers()
    {
        CreateMap<FoodCreateCommand, Food>();
        CreateMap<FoodCreateDto, Food>();

        CreateMap<Food, FoodGetDto>()
             .ForMember(x => x.CategoryDisplay,
                 src => src.MapFrom(x => x.Category.HasValue ? x.Category.GetDescription() : null))
             .ForMember(x => x.Orders,
                src => src.MapFrom(x => x.FoodOrderRelations.Select(x => x.Order)))    
             .ReverseMap();
             
        CreateMap<Food, FoodListDto>()
             .ForMember(x => x.CategoryDisplay,
                 src => src.MapFrom(x => x.Category.HasValue ? x.Category.GetDescription() : null))
             .ForMember(x => x.Orders,
                src => src.MapFrom(x => x.FoodOrderRelations.Select(x => x.Order)))    
             .ReverseMap();

        CreateMap<Food, FoodsRelatedsDto>()
             .ForMember(x => x.CategoryDisplay,
                 src => src.MapFrom(x => x.Category.HasValue ? x.Category.GetDescription() : null))
             .ReverseMap();
    }
}
