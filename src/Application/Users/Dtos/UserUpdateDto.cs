using Application.Orders.Dtos;
using Domain.Models;

namespace Application.Users.Dtos
{
    #nullable disable
    public class UserUpdateDto
    {
        public string Name { get; set; }
        public AddressUpdateDto Address { get; set; }
    }
}