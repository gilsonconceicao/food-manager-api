using AutoMapper;
using FoodManager.Domain.Models;
using FoodManager.Application.Users.Dtos;
using FoodManager.Application.Users.Commands;

namespace FoodManager.Application.Mappings;

public class UserMappers : Profile
{
    public UserMappers()
    {
        CreateMap<AddressCreateDto, Address>();
        CreateMap<UserCreateCommand, User>();
        CreateMap<User, GetUserDto>()
            .ReverseMap();
        CreateMap<User, UserRelatedOrderDto>(); 
    }
}
