using AutoMapper;
using Application.Foods.Commands;
using Application.Foods.Queries.GetAllWithPaginationFoodQuery;
using Domain.Models;
using Application.Foods.Commands.Dtos;
using Application.Utils;
using Domain.Enums;

namespace Application.Mappings;

public class FoodMappers : Profile
{
    public FoodMappers()
    {
        CreateMap<FoodCreateCommand, Food>();
        CreateMap<FoodCreateDto, Food>();

        CreateMap<Food, FoodDto>()
              .ForMember(x => x.CategoryDisplay,
                    src => src.MapFrom(x => x.Category.HasValue ? x.Category.GetDescription() : null))
              .ForMember(
                    x => x.Category,
                    src => src.MapFrom(x => x.Category.HasValue ? Enum.GetName(typeof(FoodCategoryEnum), x.Category.GetValueOrDefault()) : null)
                )
              .ForMember(x => x.Orders, src => src.MapFrom(x => x.Items.Select(x => x.Order)))
              .ForMember(x => x.Url, src => src.MapFrom(x => x.UrlImage));

        CreateMap<Food, FoodItemsDto>()
             .ForMember(x => x.CategoryDisplay,
                 src => src.MapFrom(x => x.Category.HasValue ? x.Category.GetDescription() : null))
             .ReverseMap();
    }
}
