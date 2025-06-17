
using FluentValidation;
using Domain.Enums;
using Domain.Common.Exceptions;
using Application.Utils;
using Infrastructure.Database;
using MediatR;
using Microsoft.AspNetCore.Http;

#nullable disable
namespace Application.Orders.Commands;

public class OrderUpdateCommand : IRequest<bool>
{
    public Guid OrderId { get; set; }
    public string Observations { get; set; }
}

public class OrderUpdateHandler : IRequestHandler<OrderUpdateCommand, bool>
{
    private readonly DataBaseContext _context;

    public OrderUpdateHandler(DataBaseContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(OrderUpdateCommand request, CancellationToken cancellationToken)
    {
        var order = _context.Orders
            .Where(o => !o.IsDeleted)
            .FirstOrDefault(o => o.Id == request.OrderId)
            ?? throw new HttpResponseException(
                StatusCodes.Status400BadRequest,
                CodeErrorEnum.NOT_FOUND_RESOURCE.ToString(),
                $"Pedido não encontrado"
            );

        if (request.Observations != null)
            order.Observations = request.Observations;

        await _context.SaveChangesAsync();

        return true;
    }
}
