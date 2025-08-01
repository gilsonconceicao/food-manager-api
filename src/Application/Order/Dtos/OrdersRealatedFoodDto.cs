using Application.Foods.Queries.GetAllWithPaginationFoodQuery;

namespace Application.Orders.Dtos;
#nullable disable
public class OrdersRealatedFoodDto
{
    public Guid Id { get; set; }
    public Guid? UserId { get; set; }
    public int OrderNumber { get; set; }
}