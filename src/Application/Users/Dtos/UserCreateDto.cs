using Application.Orders.Dtos;
using Domain.Models;

namespace Application.Users.Dtos
{
    #nullable disable
    public class UserCreateDto
    {
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public AddressUpdateDto Address { get; set; }
    }
}