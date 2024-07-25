using AutoMapper;
using FoodManager.Application.Foods.Commands.CreateFoodCommand;
using FoodManager.Application.Foods.Queries.GetAllWithPaginationFoodQuery;
using FoodManager.Domain.Models;

namespace FoodManager.Application.Mappings;
public class Mappings : Profile
{
    public Mappings()
    {
        CreateMap<CreateFoodCommand, Food>();


        CreateMap<Food, GetAllWithPaginationModel>()
            .ReverseMap();

    }
}
