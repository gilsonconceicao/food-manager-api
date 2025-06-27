using AutoMapper;
using Domain.Models;
using Application.Users.Dtos;
using Application.Users.Commands;
using FirebaseAdmin.Auth;

namespace Application.Mappings;

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

         CreateMap<ExportedUserRecord, User>()
            .ForMember(x => x.Name, src => src.MapFrom(x => x.DisplayName)) 
            .ForMember(x => x.FirebaseUserId, src => src.MapFrom(x => x.Uid)) 
            .ForMember(x => x.Email, src => src.MapFrom(x => x.Email)) 
            .ForMember(x => x.PhoneNumber, src => src.MapFrom(x => x.PhoneNumber));  
    }
}
