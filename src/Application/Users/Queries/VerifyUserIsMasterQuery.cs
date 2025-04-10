using Infrastructure.Database;
using MediatR;
#nullable disable 
namespace Application.Users.Queries;

public class VerifyUserIsMasterQuery : IRequest<bool>
{
    public string FirebaseUserId { get; set; }
}

public class VerifyUserIsMasterQueryHandler : IRequestHandler<VerifyUserIsMasterQuery, bool>
{
    private readonly DataBaseContext _context;

    public VerifyUserIsMasterQueryHandler(DataBaseContext context)
    {
        _context = context;

    }
    public Task<bool> Handle(VerifyUserIsMasterQuery request, CancellationToken cancellationToken)
    {
        var userIsMaster = _context.Users.Any(c => c.FirebaseUserId == request.FirebaseUserId && c.IsRoot == true);
        return Task.FromResult(userIsMaster);
    }
}