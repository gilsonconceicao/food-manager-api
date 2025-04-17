namespace Application.Workflows.Workflows;

public interface ITryHangFireWorkflow
{
    Task TryAsync();
}

public class TryHangFireWorkflow : ITryHangFireWorkflow
{
    private readonly IProcessMergeUsersFirebaseActivity _processMergeUsersFirebaseActivity;
    public TryHangFireWorkflow(IProcessMergeUsersFirebaseActivity activity)
    {
        _processMergeUsersFirebaseActivity = activity;
    }
    public async Task TryAsync()
    {
        await _processMergeUsersFirebaseActivity.ExecuteAsync();
    }
}