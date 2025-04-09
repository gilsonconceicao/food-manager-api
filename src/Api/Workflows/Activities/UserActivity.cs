using AutoMapper;
using Infrastructure.Database;
using MediatR;

namespace Api.Workflows.Workflows;

public interface IUserActivity
{
    Task ProcessCreateUser();
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
}
