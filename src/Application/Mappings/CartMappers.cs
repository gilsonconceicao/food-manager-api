using AutoMapper;
using Application.Foods.Commands;
using Application.Foods.Queries.GetAllWithPaginationFoodQuery;
using Domain.Models;
using Application.Foods.Commands.Dtos;
using Application.Utils;
using Domain.Enums;
using Application.Carts.Dtos;

namespace Application.Mappings;

public class CartMappers : Profile
{
    public CartMappers()
    {
        CreateMap<Cart, CartDto>().ReverseMap();
    }
}
