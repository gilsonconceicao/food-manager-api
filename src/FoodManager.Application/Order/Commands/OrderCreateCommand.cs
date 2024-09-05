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

            var orderCount = await _context.Orders.CountAsync(cancellationToken);

            Order order = _mapper.Map<OrderCreateCommand, Order>(request);

            order.RequestNumber = orderCount + 1;
            // if (order.Client.Address is not null && order.Client is not null)
            // {
            //     order.Client.Address.ClientId = order.Client.Id;
            // };

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
