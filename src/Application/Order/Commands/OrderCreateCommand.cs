
using FluentValidation;
using Api.Enums;
using Application.Common.Exceptions;
using Application.Utils;
using Domain.Enums;
using Domain.Models;
using Infrastructure.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;

#nullable disable
namespace Application.Orders.Commands;

public class OrderCreateCommand : IRequest<Guid>
{
    public string UserId { get; set; }
    public List<Guid> CartIds { get; set; }
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
            ?? throw new HttpResponseException
            {
                Status = 404,
                Value = new
                {
                    Code = CodeErrorEnum.NOT_FOUND_RESOURCE.ToString(),
                    Message = "Usuário não encontrado",
                }
            };

        var orderCount = await _context.Orders.CountAsync(cancellationToken);

        Order order = new Order
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            RequestNumber = orderCount + 1,
            CreatedAt = DateTime.UtcNow,
            Status = OrderStatus.AwaitingPayment
        };

        await _context.Orders.AddAsync(order, cancellationToken);

        var cartIds = request.CartIds;

        var getCarts = await _context.Carts
            .AsNoTracking()
            .Include(c => c.Food)
            .Where(x => cartIds.Contains(x.Id))
            .Where(x => x.CreatedByUserId == user.FirebaseUserId)
            .ToListAsync();

        var newOrderItems = new List<OrderItems>();
        decimal totalValue = 0;

        foreach (var item in getCarts)
        {
            var orderItem = new OrderItems
            {
                OrderId = order.Id,
                FoodId = item.FoodId,
                Quantity = item.Quantity,
                Price = item.Food.Price
            };

            totalValue = item.Food.Price + totalValue; 

            newOrderItems.Add(orderItem);
        }

        order.TotalValue = totalValue;
        
        await _context.Items.AddRangeAsync(newOrderItems);

        _context.Carts.RemoveRange(getCarts);
        await _context.SaveChangesAsync();

        return order.Id;
    }
}
