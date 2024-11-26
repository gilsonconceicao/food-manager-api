using Api.Enums;
using Api.Services;
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
    private readonly IHttpUserService _httpUserService;

    private readonly List<string> EmailsRoot = new List<string>() { "gilsonconceicaosantos.jr@gmail.com", "crislaureano01@gmail.com", "jamileoliver21@gmail.com" };
    public VerifyUserIsMasterQueryHandler(
        DataBaseContext context,
        IHttpUserService httpUserService)
    {
        _context = context;
        _httpUserService = httpUserService;

    }
    public async Task<bool> Handle(VerifyUserIsMasterQuery request, CancellationToken cancellationToken)
    {

        var user = _httpUserService.GetUserByUserIdAsync(request?.FirebaseUserId);
        var result = user.Result;
        
        if (EmailsRoot.Contains(result.Email))
            return true;

        return false;
    }
}