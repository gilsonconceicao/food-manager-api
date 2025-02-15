using AutoMapper;
using FluentValidation;
using Api.Enums;
using Application.Common.Exceptions;
using Application.Users.Dtos;
using Application.Utils;
using Domain.Extensions;
using Domain.Models;
using Infrastructure.Database;
using MediatR;

namespace Application.Users.Commands;
#nullable disable
public class UserCreateCommand : IRequest<User>
{
    public string Name { get; set; }
    public string PhoneNumber { get; set; }
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
        var validationResult = _validator.Validate(request);

        if (!validationResult.IsValid)
            ErrorUtils.InvalidFieldsError(validationResult);

        User user = _mapper.Map<User>(request);
        user.PhoneNumber = user.PhoneNumber.RemoveSpecialCharacters();
        if (request.Address != null)
        {
            user.Address.UserId = user.Id;
        };

        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
        return user;
    }
}
