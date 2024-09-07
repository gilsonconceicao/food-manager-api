using FluentValidation;
using FoodManager.API.Enums;
using FoodManager.Application.Common.Exceptions;
using FoodManager.Application.Users.Dtos;
using FoodManager.Application.Utils;
using FoodManager.Infrastructure.Database;
using MediatR;

namespace FoodManager.Application.Users.Commands;
#nullable disable
public class UserCreateCommand : IRequest<Guid>
{
    public string Name { get; set; }
    public string RegistrationNumber { get; set; }
    public AddressCreateDto? Address { get; set; }
}

public class UserCreateCommandHandler : IRequestHandler<UserCreateCommand, Guid>
{
    private readonly DataBaseContext _context;
    private readonly IValidator<UserCreateCommand> _validator;

    public UserCreateCommandHandler(DataBaseContext context,
        IValidator<UserCreateCommand> validator
    )
    {
        this._context = context;
        this._validator = validator;
    }

    public async Task<Guid> Handle(UserCreateCommand request, CancellationToken cancellationToken)
    {
        var validationResult = _validator.Validate(request);

        if (!validationResult.IsValid)
            ErrorUtils.InvalidFieldsError(validationResult);

        var teste = Guid.NewGuid();
        return teste;
    }
}
