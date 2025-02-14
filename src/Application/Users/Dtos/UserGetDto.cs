using Application.Orders.Dtos;
using Domain.Models;

namespace Application.Users.Dtos
{
    #nullable disable
    public class GetUserDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public AddressGetDto Address { get; set; }
        public ICollection<OrdersRealatedFoodDto> Orders { get; set; }
    }
}