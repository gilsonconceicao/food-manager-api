using AutoMapper;
using FluentValidation;
using FoodManager.API.Enums;
using FoodManager.Application.Common.Exceptions;
using FoodManager.Application.Users.Dtos;
using FoodManager.Application.Utils;
using FoodManager.Domain.Models;
using FoodManager.Infrastructure.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FoodManager.Application.Users.Commands;
#nullable disable
public class UserUpdateCommand : IRequest<bool>
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public AddressUpdateDto? Address { get; set; }
}

public class UserUpdateCommandHandler : IRequestHandler<UserUpdateCommand, bool>
{
    private readonly DataBaseContext _context;
    private readonly IValidator<UserUpdateCommand> _validator;
    private readonly IMapper _mapper;

    public UserUpdateCommandHandler(DataBaseContext context,
        IValidator<UserUpdateCommand> validator,
        IMapper mapper
    )
    {
        this._context = context;
        this._validator = validator;
        this._mapper = mapper;
    }

    public async Task<bool> Handle(UserUpdateCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var validationResult = _validator.Validate(request);

            if (!validationResult.IsValid)
                ErrorUtils.InvalidFieldsError(validationResult);

            var user = _context.Users
                .Include(x => x.Address)
                .Where(x => !x.IsDeleted)
                .FirstOrDefault(x => x.Id == request.Id);

            if (user == null)
            {
                throw new HttpResponseException
                {
                    Status = 404,
                    Value = new
                    {
                        Code = CodeErrorEnum.NOT_FOUND_RESOURCE.ToString(),
                        Message = $"Usuário não encontrado",
                        Resource = request.Id
                    }
                };
            }

            if (user.Address == null && request.Address != null)
            {
                Address newAddress = _mapper.Map<Address>(request.Address);
                newAddress.UserId = user.Id;
                _context.Address.Add(newAddress);
            }
            else if (request.Address != null)
            {
                user.Address.ZipCode = request.Address.ZipCode;
                user.Address.City = request.Address.City;
                user.Address.Number = request.Address.Number;
                user.Address.State = request.Address.State;
                user.Address.Street = request.Address.Street;
            };

            user.Name = request.Name;
            user.Email = request.Email;
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
