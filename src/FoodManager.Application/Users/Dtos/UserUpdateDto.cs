using FoodManager.Application.Orders.Dtos;
using FoodManager.Domain.Models;

namespace FoodManager.Application.Users.Dtos
{
    #nullable disable
    public class UserUpdateDto
    {
        public string Name { get; set; }
        public AddressUpdateDto Address { get; set; }
    }
}