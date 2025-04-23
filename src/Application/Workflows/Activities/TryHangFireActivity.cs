using Application.Carts.Commands;
using MediatR;

namespace Application.Workflows.Workflows;

public interface IProcessTryHangFireActivity
{
    Task ExecuteAsync();
}

public class ProcessTryHangFireActivity : IProcessTryHangFireActivity
{
    private readonly IMediator _mediator;

    public ProcessTryHangFireActivity(
        IMediator mediator
    )
    {
        _mediator = mediator;
    }

    public async Task ExecuteAsync()
    {
        await _mediator.Send(new TryHangFireCommand{});
    }
}
