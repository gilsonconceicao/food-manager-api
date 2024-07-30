namespace FoodManager.Application.FoodOrders.Commands.Dtos;

public class AddressCreateDto
{
    public string Street { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string ZipCode { get; set; }
}