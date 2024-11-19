using Api.Enums;
using Application.Common.Exceptions;
using Domain.Models;
using Infrastructure.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Orders.Queries;

public class OrderGetByIdQuery : IRequest<Order>
{
    public Guid OrderId { get; set; }
    public OrderGetByIdQuery(Guid OrderId)
    {
        this.OrderId = OrderId;
    }
}

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
            .Include(x => x.User)
            .Include(x => x.Items)
            .ThenInclude(x => x.Food)
            .Where(x => !x.IsDeleted)
            .FirstOrDefaultAsync(x => x.Id == request.OrderId)
            ?? throw new NotFoundException("Pedido não encontrado ou não existe.");


        return order;
    }
}