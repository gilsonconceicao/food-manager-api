using FoodManager.Application.Orders.Dtos;
using FoodManager.Domain.Models;

namespace FoodManager.Application.Users.Dtos
{
    #nullable disable
    public class GetUserDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string RegistrationNumber { get; set; }
        public AddressGetDto Address { get; set; }
        public ICollection<OrderGetDto> Orders { get; set; }
    }
}