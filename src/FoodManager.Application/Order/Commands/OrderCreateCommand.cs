using AutoMapper;
using FluentValidation;
using FoodManager.API.Enums;
using FoodManager.Application.Common.Exceptions;
using FoodManager.Application.Utils;
using FoodManager.Domain.Models;
using FoodManager.Infrastructure.Database;
using FoodManager.Infrastructure.Migrations;
using MediatR;
using Microsoft.EntityFrameworkCore;

#nullable disable
namespace FoodManager.Application.Orders.Commands;
public class OrderCreateCommand : IRequest<bool>
{
    public Guid UserId { get; set; }
    public List<Guid> FoodsIds { get; set; }
}

public class OrderCreateHandler : IRequestHandler<OrderCreateCommand, bool>
{
    private readonly DataBaseContext _context;
    private readonly IValidator<OrderCreateCommand> _validator;
    private readonly IMapper _mapper;


    public OrderCreateHandler(DataBaseContext context,
    IMapper mapper, IValidator<OrderCreateCommand> validator)
    {
        _context = context;
        _mapper = mapper;
        _validator = validator;
    }

    public async Task<bool> Handle(OrderCreateCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var validationResult = _validator.Validate(request);

            if (!validationResult.IsValid)
                ErrorUtils.InvalidFieldsError(validationResult);


            var user = _context.Users
                .Where(user => !user.IsDeleted)
                .FirstOrDefault(user => user.Id == request.UserId)
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
            };

            await _context.Orders.AddAsync(order, cancellationToken);

            var getFoodIncludeIds = await _context.Foods
                .Where(x => request.FoodsIds.Contains(x.Id))
                .Select(x => x.Id)
                .ToListAsync();

            var missingFoodIds = request.FoodsIds.Except(getFoodIncludeIds).ToList();

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

            foreach (var foodId in request.FoodsIds)
            {
                var orderFoodRelation = new OrderItems
                {
                    OrderId = order.Id,
                    FoodId = foodId
                };

                _context.Set<OrderItems>().Add(orderFoodRelation);
            };

            await _context.SaveChangesAsync();
            return true;
        }
        catch (HttpResponseException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }
}
