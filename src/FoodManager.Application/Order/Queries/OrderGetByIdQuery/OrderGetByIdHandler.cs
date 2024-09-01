using FoodManager.API.Enums;
using FoodManager.Application.Common.Exceptions;
using FoodManager.Domain.Models;
using FoodManager.Infrastructure.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FoodManager.Application.Orders.Queries.OrderGetByIdQuery;

public class OrderGetByIdHandler : IRequestHandler<OrderGetByIdQuery, Order>
{
    private readonly DataBaseContext _context;
    public OrderGetByIdHandler(DataBaseContext context)
    {
        _context = context;
    }

    public async Task<Order> Handle(OrderGetByIdQuery request, CancellationToken cancellationToken)
    {
        var order = await _context.Orders
            .Include(x => x.OrdersFoodsRelationship)
            .ThenInclude(x => x.Food)
            .Where(x => !x.IsDeleted)
            .FirstOrDefaultAsync(x => x.Id == request.OrderId)
            ?? throw new HttpResponseException
                {
                    Status = 404,
                    Value = new
                    {
                        Code = CodeErrorEnum.NOT_FOUND_RESOURCE.ToString(),
                        Message = "Pedido não encontrada ou não existe",
                    }
                };

            return order;
    }
}