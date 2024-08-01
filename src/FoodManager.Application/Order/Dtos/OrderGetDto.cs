using FoodManager.Application.Foods.Queries.GetAllWithPaginationFoodQuery;
using FoodManager.Application.orders.Dtos;

namespace FoodManager.Application.Orders.Dtos;
#nullable disable
public class OrderGetDto
{
    public Guid Id { get; set; }
    public int RequestNumber { get; set; }
    public ClientGetDto Client { get; set; }
    public List<GetFoodModel> Foods { get; set; }
}