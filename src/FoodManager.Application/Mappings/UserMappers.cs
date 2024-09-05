using AutoMapper;
using FoodManager.Domain.Models;
using FoodManager.Application.Users.Dtos;

namespace FoodManager.Application.Mappings;

public class UserMappers : Profile
{
    public UserMappers()
    {
        // CreateMap<AddressCreateDto, Address>();

        CreateMap<User, GetUserDto>()
            .ReverseMap();
    }
}
