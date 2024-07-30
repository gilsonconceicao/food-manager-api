using AutoMapper;
using FluentValidation;
using FoodManager.API.Enums;
using FoodManager.Application.Common.Exceptions;
using FoodManager.Application.Utils;
using FoodManager.Domain.Models;
using FoodManager.Infrastructure.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FoodManager.Application.FoodOrders.Commands.CreateFoodOrderCommand;
public class CreateFoodOrderHandler : IRequestHandler<CreateFoodOrderCommand, bool>
{
    private readonly DataBaseContext _context;
    private readonly IValidator<CreateFoodOrderCommand> _validator;
    private readonly IMapper _mapper;


    public CreateFoodOrderHandler(DataBaseContext context,
    IMapper mapper, IValidator<CreateFoodOrderCommand> validator)
    {
        _context = context;
        _mapper = mapper;
        _validator = validator;
    }

    public async Task<bool> Handle(CreateFoodOrderCommand request, CancellationToken cancellationToken)
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

            var orderCount = await _context.FoodOrders.CountAsync();
            
            FoodOrder orderRequested = _mapper.Map<CreateFoodOrderCommand, FoodOrder>(request);
            await _context.FoodOrders.AddAsync(orderRequested);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (HttpResponseException ex)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }
}
