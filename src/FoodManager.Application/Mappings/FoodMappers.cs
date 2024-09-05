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

       CreateMap<Food, GetFoodDto>()
            .ForMember(x => x.CategoryDisplay,
                src => src.MapFrom(x => x.Category.HasValue ? x.Category.GetDescription() : null))
            .ReverseMap();
    }
}
