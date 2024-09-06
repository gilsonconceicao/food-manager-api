using AutoMapper;
using FluentValidation;
using FoodManager.API.Enums;
using FoodManager.Application.Common.Exceptions;
using FoodManager.Application.Utils;
using FoodManager.Domain.Models;
using FoodManager.Infrastructure.Database;
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
            {
                throw new HttpResponseException
                {
                    Status = 400,
                    Value = new
                    {
                        Code = CodeErrorEnum.INVALID_FORM_FIELDS.ToString(),
                        Message = "Erro ao validar campos",
                        Details = ErrorUtils.ValidationFailure(validationResult.Errors)
                    }
                };
            }

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

            var foods = await _context.Foods
                .Where(x => request.FoodsIds.Contains(x.Id))
                .ToListAsync();

            var missingFoodIds = request.FoodsIds.Except(foods.Select(f => f.Id)).ToList();

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

            var orderCount = await _context.Orders.CountAsync(cancellationToken);

            Order order = new Order
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                Foods = foods,
                User = user,
                RequestNumber = orderCount + 1,
                CreatedAt = DateTime.UtcNow,
            };

            await _context.Orders.AddAsync(order, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            _context.Orders.Update(order);
            await _context.SaveChangesAsync(cancellationToken);
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
