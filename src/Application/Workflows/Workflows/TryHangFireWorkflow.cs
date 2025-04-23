namespace Application.Workflows.Workflows;

public interface ITryHangFireWorkflow
{
    Task TryAsync();
}

public class TryHangFireWorkflow : ITryHangFireWorkflow
{
    private readonly IProcessTryHangFireActivity _processTryHangFireActivity;
    public TryHangFireWorkflow(IProcessTryHangFireActivity processTryHangFireActivity)
    {
        _processTryHangFireActivity = processTryHangFireActivity;
    }
    public async Task TryAsync()
    {
        await _processTryHangFireActivity.ExecuteAsync();
    }
}