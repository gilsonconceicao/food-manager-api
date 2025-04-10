using Application.Carts.Commands;
using AutoMapper;
using Infrastructure.Database;
using MediatR;
using Application.Users.Commands;

namespace Api.Workflows.Workflows;

public interface IUserActivity
{
    Task ProcessCreateUser();
    Task ProcessMergeUsersFirebaseAsyc();
}

public class UserActivity : IUserActivity
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private readonly DataBaseContext _context;

    public UserActivity(
        IMediator mediator,
        IMapper mapper,
        DataBaseContext db
    )
    {
        _mapper = mapper;
        _mediator = mediator;
        _context = db;
    }

    public Task ProcessCreateUser()
    {
        throw new NotImplementedException();
    }

    public async Task ProcessMergeUsersFirebaseAsyc()
    {
       await _mediator.Send(new MergeUsersFirebaseCommand{});
    }
}
