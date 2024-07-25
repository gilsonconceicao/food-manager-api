using System.Text.Json.Serialization;
using AutoMapper;
using FluentValidation;
using FoodManager.API.Enums;
using FoodManager.Application.Common.Exceptions;
using FoodManager.Application.Utils;
using FoodManager.Domain.Models;
using FoodManager.Infrastructure.Database;
using MediatR;

namespace FoodManager.Application.Foods.Commands.CreateFoodCommand;


public class CreateFoodHandler : IRequestHandler<CreateFoodCommand, bool>
{
    private readonly IMapper _mapper;
    private readonly DataBaseContext _context;
    private readonly IValidator<CreateFoodCommand> _validator;

    public CreateFoodHandler(IMapper mapper, DataBaseContext context, IValidator<CreateFoodCommand> validator)
    {
        _mapper = mapper;
        _context = context;
        _validator = validator;
    }

    public async Task<bool> Handle(CreateFoodCommand request, CancellationToken cancellationToken)
    {

        var validationResult = _validator.Validate(request);

        if (!validationResult.IsValid)
        {
            throw new HttpResponseException
            {
                Status = 400, 
                Value = new {
                    Code = CodeErrorEnum.INVALID_FORM_FIELDS.ToString(),
                    Message = "Erro ao validar campos", 
                    Details = ErrorUtils.ValidationFailure(validationResult.Errors)
                }
            };
        }

        try
        {
            Food food = _mapper.Map<Food>(request);
            await _context.Foods.AddAsync(food); 
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

}
