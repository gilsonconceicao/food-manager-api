namespace FoodManager.Application.FoodsOrders.Commands.Dtos;
#nullable disable
public class ClientCreateDto
{
    public string Name { get; set; }
    public string DocumentNumber { get; set; }
    public string PhoneNumber { get; set; }
    public AddressCreateDto Address { get; set; }
}