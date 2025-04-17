namespace Application.Workflows.Workflows;

public interface IMergeUsersWorkflow
{
    Task MergeFirebaseUsersAsync();
}

public class MergeUsersWorkflow : IMergeUsersWorkflow
{
    private readonly IProcessMergeUsersFirebaseActivity _processMergeUsersFirebaseActivity;
    public MergeUsersWorkflow(IProcessMergeUsersFirebaseActivity activity)
    {
        _processMergeUsersFirebaseActivity = activity;
    }
    public async Task MergeFirebaseUsersAsync()
    {
        await _processMergeUsersFirebaseActivity.ExecuteAsync();
    }
}