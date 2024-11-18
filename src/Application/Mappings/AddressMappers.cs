using AutoMapper;
using Application.Users.Dtos;
using Domain.Models;

namespace Application.Mappings; 

public class AddressMappers : Profile
{
    public AddressMappers()
    {
        CreateMap<Address, AddressGetDto>();
        CreateMap<AddressCreateDto, Address>();
    }
}