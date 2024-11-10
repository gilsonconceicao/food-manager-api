using AutoMapper;
using FluentValidation;
using FoodManager.API.Enums;
using FoodManager.Application.Common.Exceptions;
using FoodManager.Application.Users.Dtos;
using FoodManager.Application.Utils;
using FoodManager.Domain.Extensions;
using FoodManager.Domain.Models;
using FoodManager.Infrastructure.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FoodManager.Application.Users.Commands;
#nullable disable
public class UserCreateCommand : IRequest<User>
{
    public string Name { get; set; }
    public string RegistrationNumber { get; set; }
    public string Email { get; set; }
    public string FirebaseUserId { get; set; }
    public AddressCreateDto? Address { get; set; }
}

public class UserCreateCommandHandler : IRequestHandler<UserCreateCommand, User>
{
    private readonly DataBaseContext _context;
    private readonly IValidator<UserCreateCommand> _validator;
    private readonly IMapper _mapper;

    public UserCreateCommandHandler(DataBaseContext context,
        IValidator<UserCreateCommand> validator,
        IMapper mapper
    )
    {
        this._context = context;
        this._validator = validator;
        this._mapper = mapper;
    }

    public async Task<User> Handle(UserCreateCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var validationResult = _validator.Validate(request);

            if (!validationResult.IsValid)
                ErrorUtils.InvalidFieldsError(validationResult);

            User user = _mapper.Map<User>(request);
            user.RegistrationNumber = user.RegistrationNumber.RemoveSpecialCharacters();
            if (request.Address != null)
            {
                user.Address.UserId = user.Id;
            };

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
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
