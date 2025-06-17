namespace Application.Workflows.Workflows;

public interface ISendEmailContactWorkflow
{
    Task SendEmailAsync(Guid contactId);
}

public class SendEmailContactWorkflow : ISendEmailContactWorkflow
{
    private readonly IProcessSendEmailContactActivity _processTryHangFireActivity;
    public SendEmailContactWorkflow(IProcessSendEmailContactActivity processTryHangFireActivity)
    {
        _processTryHangFireActivity = processTryHangFireActivity;
    }
    public async Task SendEmailAsync(Guid contactId)
    {
        await _processTryHangFireActivity.ExecuteAsync(contactId);
    }
}