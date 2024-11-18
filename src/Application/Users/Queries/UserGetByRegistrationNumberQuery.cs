using Api.Enums;
using Application.Common.Exceptions;
using Domain.Extensions;
using Domain.Models;
using Infrastructure.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;

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
            .FirstOrDefaultAsync(x => x.RegistrationNumber == request.RegistrationNumber.RemoveSpecialCharacters())
            ?? throw new HttpResponseException
            {
                Status = 404,
                Value = new
                {
                    Code = CodeErrorEnum.NOT_FOUND_RESOURCE.ToString(),
                    Message = "Usuário não encontrado ou não existe",
                }
            };

        return user;
    }
}