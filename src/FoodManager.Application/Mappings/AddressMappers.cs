using AutoMapper;
using FoodManager.Application.Users.Dtos;
using FoodManager.Domain.Models;

namespace FoodManager.Application.Mappings; 

public class AddressMappers : Profile
{
    public AddressMappers()
    {
        CreateMap<Address, AddressGetDto>();
        CreateMap<AddressCreateDto, Address>();
    }
}