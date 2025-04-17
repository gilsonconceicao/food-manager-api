using Application.Carts.Commands;
using MediatR;

namespace Application.Workflows.Workflows;

public interface IProcessMergeUsersFirebaseActivity
{
    Task ExecuteAsync();
}

public class ProcessMergeUsersFirebaseActivity : IProcessMergeUsersFirebaseActivity
{
    private readonly IMediator _mediator;

    public ProcessMergeUsersFirebaseActivity(
        IMediator mediator
    )
    {
        _mediator = mediator;
    }

    public async Task ExecuteAsync()
    {
        await _mediator.Send(new MergeUsersFirebaseCommand{});
    }
}
