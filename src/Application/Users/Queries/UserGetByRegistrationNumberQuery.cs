using Domain.Enums;
using Domain.Common.Exceptions;
using Domain.Extensions;
using Domain.Models;
using Infrastructure.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace Application.Users.Queries;
#nullable disable
public class UserGetByRegistrationNumberQuery : IRequest<User>
{
    public string RegistrationNumber { get; set; }
}

public class UserGetByRegistrationNumberHandler : IRequestHandler<UserGetByRegistrationNumberQuery, User>
{
    private readonly DataBaseContext _context;
    public UserGetByRegistrationNumberHandler(DataBaseContext context)
    {
        _context = context;
    }
    public async Task<User> Handle(UserGetByRegistrationNumberQuery request, CancellationToken cancellationToken)
    {

        User user = await _context.Users
            .Include(x => x.Address)
            .Include(x => x.Orders)
            .Where(x => !x.IsDeleted)
            .FirstOrDefaultAsync(x => x.PhoneNumber == request.RegistrationNumber.RemoveSpecialCharacters())
            ?? throw new HttpResponseException(
                StatusCodes.Status404NotFound,
                CodeErrorEnum.NOT_FOUND_RESOURCE.ToString(),
                $"Usuário não encontrado");

        return user;
    }
}