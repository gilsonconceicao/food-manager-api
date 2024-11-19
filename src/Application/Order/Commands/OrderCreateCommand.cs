
using FluentValidation;
using Api.Enums;
using Application.Common.Exceptions;
using Application.Orders.Dtos;
using Application.Utils;
using Domain.Enums;
using Domain.Models;
using Infrastructure.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;

#nullable disable
namespace Application.Orders.Commands;
public class OrderCreateCommand : IRequest<bool>
{
    public string UserId { get; set; }
    public List<OrderItemCreateDto> Foods { get; set; }
}

public class OrderCreateHandler : IRequestHandler<OrderCreateCommand, bool>
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

    public async Task<bool> Handle(OrderCreateCommand request, CancellationToken cancellationToken)
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
            Status = OrderStatus.Created
        };

        await _context.Orders.AddAsync(order, cancellationToken);

        var foodIdsRequest = request.Foods.Select(x => x.FoodId).ToList();

        var getFoodIncludeIds = await _context.Foods
            .Where(x => foodIdsRequest.Contains(x.Id))
            .Select(x => x.Id)
            .ToListAsync();

        var missingFoodIds = foodIdsRequest.Except(getFoodIncludeIds).ToList();

        if (missingFoodIds.Any())
        {
            throw new HttpResponseException
            {
                Status = 404,
                Value = new
                {
                    Code = CodeErrorEnum.NOT_FOUND_RESOURCE.ToString(),
                    Message = $"Comidas não foram encontradas",
                    Resource = missingFoodIds
                }
            };
        };

        foreach (var item in request.Foods)
        {
            var orderFoodRelation = new OrderItems
            {
                OrderId = order.Id,
                FoodId = item.FoodId,
                Quantity = item.Quantity ?? null,
                Observations = item.Observations ?? null
            };

            _context.Set<OrderItems>().Add(orderFoodRelation);
        };

        await _context.SaveChangesAsync();
        return true;
    }
}
