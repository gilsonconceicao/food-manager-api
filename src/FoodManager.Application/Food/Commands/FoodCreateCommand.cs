using System.Text.Json.Serialization;
using AutoMapper;
using FluentValidation;
using FoodManager.API.Enums;
using FoodManager.Application.Common.Exceptions;
using FoodManager.Application.Utils;
using FoodManager.Domain.Enums;
using FoodManager.Domain.Models;
using FoodManager.Infrastructure.Database;
using MediatR;
#nullable disable
namespace FoodManager.Application.Foods.Commands;

public class FoodCreateCommand : IRequest<bool>
    {
        public string Name { get; set; }
        public string UrlImage { get; set; }
        public string Description { get; set; }
        public bool IsAvailable { get; set; }
        public decimal Price { get; set; }
        public FoodCategoryEnum Category { get; set; }
        public string PreparationTime { get; set; }
    }

public class FoodCreateHandler : IRequestHandler<FoodCreateCommand, bool>
{
    private readonly IMapper _mapper;
    private readonly DataBaseContext _context;
    private readonly IValidator<FoodCreateCommand> _validator;

    public FoodCreateHandler(IMapper mapper, DataBaseContext context, IValidator<FoodCreateCommand> validator)
    {
        _mapper = mapper;
        _context = context;
        _validator = validator;
    }

    public async Task<bool> Handle(FoodCreateCommand request, CancellationToken cancellationToken)
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
            
            Food food = _mapper.Map<Food>(request);
            await _context.Foods.AddAsync(food);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (HttpResponseException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

}
