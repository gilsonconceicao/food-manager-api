using FoodManager.API.Enums;
using FoodManager.Application.Common.Exceptions;
using FoodManager.Domain.Models;
using FoodManager.Infrastructure.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FoodManager.Application.Users.Queries;

public class UserGetByIdQuery : IRequest<User>
{
    public Guid Id { get; set; }
}

public class UserGetByIdQueryHandler : IRequestHandler<UserGetByIdQuery, User>
{
    private readonly DataBaseContext _context;
    public UserGetByIdQueryHandler(DataBaseContext context)
    {
        _context = context;
    }
    public async Task<User> Handle(UserGetByIdQuery request, CancellationToken cancellationToken)
    {

        User user = await _context.Users
            .Include(x => x.Address)
            .Include(x => x.Orders)
            .Where(x => !x.IsDeleted)
            .FirstOrDefaultAsync(x => x.Id == request.Id)
            ?? throw new HttpResponseException
            {
                Status = 404,
                Value = new
                {
                    Code = CodeErrorEnum.NOT_FOUND_RESOURCE.ToString(),
                    Message = "Comida não encontrada ou não existe",
                }
            };

        return user;
    }
}