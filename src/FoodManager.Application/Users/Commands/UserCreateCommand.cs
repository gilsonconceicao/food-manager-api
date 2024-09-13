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

    public async Task<Guid> Handle(UserCreateCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var validationResult = _validator.Validate(request);

            if (!validationResult.IsValid)
                ErrorUtils.InvalidFieldsError(validationResult);

            var userByRegistrationNumber = _context.Users
                .Include(x => x.Address)
                .Where(x => !x.IsDeleted)
                .FirstOrDefault(x => x.RegistrationNumber == request.RegistrationNumber.RemoveSpecialCharacters());

            if (userByRegistrationNumber != null)
            {
                throw new HttpResponseException
                {
                    Status = 400,
                    Value = new
                    {
                        Code = CodeErrorEnum.INVALID_FORM_FIELDS.ToString(),
                        Message = $"CPF informado já existe", 
                        Resource = request.RegistrationNumber
                    }
                };
            }

            User user = _mapper.Map<User>(request);
            user.RegistrationNumber = user.RegistrationNumber.RemoveSpecialCharacters();
            if (request.Address != null)
            {
                user.Address.UserId = user.Id;
            };

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user.Id;
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
