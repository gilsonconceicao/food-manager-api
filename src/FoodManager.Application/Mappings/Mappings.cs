using AutoMapper;
using FoodManager.Application.Foods.Commands.CreateFoodCommand;
using FoodManager.Application.Foods.Queries.GetAllWithPaginationFoodQuery;
using FoodManager.Domain.Extensions;
using FoodManager.Domain.Models;

#nullable disable
namespace FoodManager.Application.Mappings;
public class Mappings : Profile
{
    public Mappings()
    {
        CreateMap<CreateFoodCommand, Food>();

        CreateMap<Food, GetFoodModel>()
            .ForMember(x => x.CategoryDisplay, src => src.MapFrom(x => x.Category.GetDescription()))
            .ReverseMap();
    }
}
