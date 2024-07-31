using FoodManager.Application.Orders.Dtos;
using FoodManager.Domain.Extensions;
using MediatR;

namespace FoodManager.Application.Orders.Queries.OrderPaginationListQuery;

public class OrderPaginationListQuery : IRequest<PagedList<OrderGetDto>>
{
    public int Page { get; set; } = 0;
    public int Size { get; set; } = 5;
}