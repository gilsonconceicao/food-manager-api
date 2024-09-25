using FoodManager.API.Enums;
using FoodManager.Application.Common.Exceptions;
using FoodManager.Domain.Extensions;
using FoodManager.Domain.Models;
using FoodManager.Infrastructure.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FoodManager.Application.Users.Queries;
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