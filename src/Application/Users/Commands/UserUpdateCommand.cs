using AutoMapper;
using FluentValidation;
using Domain.Enums;
using Domain.Common.Exceptions;
using Application.Users.Dtos;
using Application.Utils;
using Domain.Models;
using Infrastructure.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace Application.Users.Commands;
#nullable disable
public class UserUpdateCommand : IRequest<bool>
{
    public string UserId { get; set; }
    public string Name { get; set; }
    public string PhoneNumber { get; set; }
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
        var validationResult = _validator.Validate(request);

        if (!validationResult.IsValid)
            ErrorUtils.InvalidFieldsError(validationResult);

        var user = _context.Users
            .Include(x => x.Address)
            .Where(x => !x.IsDeleted)
            .FirstOrDefault(x => x.FirebaseUserId == request.UserId);

        if (user == null)
        {
            throw new HttpResponseException(
               StatusCodes.Status400BadRequest,
               CodeErrorEnum.NOT_FOUND_RESOURCE.ToString(),
               $"Usuário não encontrado"
           );
        }

        if (user.Address == null && request.Address != null)
        {
            Address newAddress = _mapper.Map<Address>(request.Address);
            newAddress.UserId = user.Id;
            await _context.Address.AddAsync(newAddress);
            user.AddressId = newAddress.Id;
        }
        else if (request.Address != null)
        {
            var currentAddress = user.Address;
            var newAddress = request.Address; 

            if (newAddress.ZipCode != null)
                currentAddress.ZipCode = newAddress.ZipCode;
            if (newAddress.City != null)
                currentAddress.City = newAddress.City;
            if (newAddress.Number != null)
                currentAddress.Number = newAddress.Number;
            if (newAddress.State != null)
                currentAddress.State = newAddress.State;
            if (newAddress.Street != null)
                currentAddress.Street = newAddress.Street;
            if (newAddress.Complement != null)
                currentAddress.Complement = newAddress.Complement;
        };

        if (request.PhoneNumber != null)
            user.PhoneNumber = request.PhoneNumber;

        if (request.Name != null)
            user.Name = request.Name;
        user.AddressId = user.Address.Id; 
        await _context.SaveChangesAsync();
        return true;
    }
}
