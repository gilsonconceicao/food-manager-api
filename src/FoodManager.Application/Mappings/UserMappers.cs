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
        CreateMap<AddressUpdateDto, Address>();
        CreateMap<UserCreateCommand, User>();
        CreateMap<User, UserRelatedOrderDto>(); 
        
        CreateMap<User, GetUserDto>()
            .ReverseMap();
        CreateMap<CreatedByDto, User>()
            .ForMember(x => x.Name, src => src.MapFrom(x => x.UserName))
            .ForMember(x => x.Id, src => src.MapFrom(x => x.UserId))
            .ForMember(x => x.RegistrationNumber, src => src.MapFrom(x => x.UserRegistrationNumber))
            .ReverseMap();


    }
}
