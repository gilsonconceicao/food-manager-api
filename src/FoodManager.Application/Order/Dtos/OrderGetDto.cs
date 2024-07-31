using FoodManager.Application.orders.Dtos;

namespace FoodManager.Application.Orders.Dtos;

public class OrderGetDto
{
    public Guid Id { get; set; }
    public int RequestNumber { get; set; }
    public ClientGetDto Client { get; set; }
    // public ICollection<Food> Foods { get; set; }s
}