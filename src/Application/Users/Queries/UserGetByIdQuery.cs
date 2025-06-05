using Domain.Enums;
using Domain.Common.Exceptions;
using Domain.Models;
using Infrastructure.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Users.Queries;

public class UserGetByIdQuery : IRequest<User>
{
    public string Id { get; set; }
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
            .FirstOrDefaultAsync(x => x.FirebaseUserId == request.Id)
            ?? throw new NotFoundException("Usuário não encontrada ou não existe.");

        return user;
    }
}