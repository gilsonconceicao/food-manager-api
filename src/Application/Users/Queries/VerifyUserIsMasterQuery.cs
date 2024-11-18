using Api.Enums;
using Application.Common.Exceptions;
using Domain.Models;
using Infrastructure.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;
#nullable disable 
namespace Application.Users.Queries;

public class VerifyUserIsMasterQuery : IRequest<bool>
{
    public string FirebaseUserId { get; set; }
}

public class VerifyUserIsMasterQueryHandler : IRequestHandler<VerifyUserIsMasterQuery, bool>
{
    private readonly DataBaseContext _context;
    private readonly List<string> EmailsRoot = new List<string>() { "gilsonconceicaosantos.jr@gmail.com", "crislaureano01@gmail.com", "jamileoliver21@gmail.com" };
    public VerifyUserIsMasterQueryHandler(DataBaseContext context)
    {
        _context = context;
    }
    public async Task<bool> Handle(VerifyUserIsMasterQuery request, CancellationToken cancellationToken)
    {

        User user = await _context.Users
            .Include(x => x.Address)
            .Include(x => x.Orders)
            .Where(x => !x.IsDeleted)
            .FirstOrDefaultAsync(x => x.FirebaseUserId == request.FirebaseUserId)
            ?? throw new HttpResponseException
            {
                Status = 404,
                Value = new
                {
                    Code = CodeErrorEnum.NOT_FOUND_RESOURCE.ToString(),
                    Message = "Usuário não encontrada ou não existe",
                }
            };


        if (EmailsRoot.Contains(user.Email))
            return true;

        return false;
    }
}