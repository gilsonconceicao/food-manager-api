
using FluentValidation;
using Domain.Enums;
using Domain.Common.Exceptions;
using Application.Utils;
using Domain.Enums;
using Domain.Models;
using Infrastructure.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

#nullable disable
namespace Application.Orders.Commands;

public class OrderCreateCommand : IRequest<Guid>
{
    public string UserId { get; set; }
    public List<Guid> CartIds { get; set; }
    public string? Observations { get; set; }
}

public class OrderCreateHandler : IRequestHandler<OrderCreateCommand, Guid>
{
    private readonly DataBaseContext _context;
    private readonly IValidator<OrderCreateCommand> _validator;

    public OrderCreateHandler(
        DataBaseContext context,
        IValidator<OrderCreateCommand> validator
    )
    {
        _context = context;
        _validator = validator;
    }

    public async Task<Guid> Handle(OrderCreateCommand request, CancellationToken cancellationToken)
    {
        var validationResult = _validator.Validate(request);

        if (!validationResult.IsValid)
            ErrorUtils.InvalidFieldsError(validationResult);

        var user = _context.Users
            .Where(user => !user.IsDeleted)
            .FirstOrDefault(user => user.FirebaseUserId == request.UserId)
            ?? throw new HttpResponseException(
                StatusCodes.Status404NotFound,
                CodeErrorEnum.NOT_FOUND_RESOURCE.ToString(),
                $"Usuário não encontrado"
            );

        var orderCount = await _context.Orders.SumAsync(x => x.RequestNumber);

        Order order = new Order
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            RequestNumber = orderCount + 1,
            CreatedAt = DateTime.UtcNow,
            Status = OrderStatus.AwaitingPayment,
            Observations = request?.Observations
        };

        var cartIds = request.CartIds;

        var getCarts = await _context.Carts
            .AsNoTracking()
            .Include(c => c.Food)
            .Where(x => cartIds.Contains(x.Id))
            .Where(x => x.CreatedByUserId == user.FirebaseUserId)
            .ToListAsync();

        if (getCarts.Any(x => x.Quantity <= 0))
        {
            throw new HttpResponseException(
                StatusCodes.Status400BadRequest,
                CodeErrorEnum.INVALID_BUSINESS_RULE.ToString(),
                $"Não é permitido seguir com item sem quantidade"
            );
        }

        await _context.Orders.AddAsync(order, cancellationToken);


        var newOrderItems = new List<OrderItems>();

        foreach (var item in getCarts)
        {
            var orderItem = new OrderItems
            {
                OrderId = order.Id,
                FoodId = item.FoodId,
                Quantity = item.Quantity,
                Price = item.Food.Price
            };


            newOrderItems.Add(orderItem);
        }

        await _context.Items.AddRangeAsync(newOrderItems);
        order.TotalValue = newOrderItems.Sum(i => i.Quantity * i.Price);

        _context.Carts.RemoveRange(getCarts);
        await _context.SaveChangesAsync();

        return order.Id;
    }
}
