using System.Text.Json.Serialization;
using AutoMapper;
using FluentValidation;
using Api.Enums;
using Application.Common.Exceptions;
using Application.Utils;
using Domain.Enums;
using Domain.Models;
using Infrastructure.Database;
using MediatR;
#nullable disable
namespace Application.Foods.Commands;

public class FoodCreateCommand : IRequest<bool>
{
    public string Name { get; set; }
    public string UrlImage { get; set; }
    public string Description { get; set; }
    public bool IsAvailable { get; set; }
    public decimal Price { get; set; }
    public FoodCategoryEnum Category { get; set; }
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
                ErrorUtils.InvalidFieldsError(validationResult);


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
