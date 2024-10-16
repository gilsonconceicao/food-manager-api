using AutoMapper;
using FoodManager.Application.Foods.Commands;
using FoodManager.Application.Foods.Queries.GetAllWithPaginationFoodQuery;
using FoodManager.Domain.Models;
using FoodManager.Application.Foods.Commands.Dtos;
using FoodManager.Application.Utils;

namespace FoodManager.Application.Mappings;

public class FoodMappers : Profile
{
    public FoodMappers()
    {
        CreateMap<FoodCreateCommand, Food>();
        CreateMap<FoodCreateDto, Food>();

        CreateMap<Food, FoodDto>()
             .ForMember(x => x.CategoryDisplay,
                 src => src.MapFrom(x => x.Category.HasValue ? x.Category.GetDescription() : null))
              .ForMember(x => x.Orders, src => src.MapFrom(x => x.Items.Select(x => x.Order)))
              .ForMember(x => x.Url, src => src.MapFrom(x => x.UrlImage));

        CreateMap<Food, FoodItemsDto>()
             .ForMember(x => x.CategoryDisplay,
                 src => src.MapFrom(x => x.Category.HasValue ?  x.Category.GetDescription() : null))
             .ReverseMap();
    }
}
