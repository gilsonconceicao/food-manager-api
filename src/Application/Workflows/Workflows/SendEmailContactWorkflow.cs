using Microsoft.Extensions.Logging;

namespace Application.Workflows.Workflows;

public interface ISendEmailContactWorkflow
{
    Task SendEmailAsync(Guid contactId);
}

public class SendEmailContactWorkflow : ISendEmailContactWorkflow
{
    private readonly IProcessSendEmailContactActivity _processTryHangFireActivity;
    private readonly ILogger<SendEmailContactWorkflow> _logger;

    public SendEmailContactWorkflow(IProcessSendEmailContactActivity processTryHangFireActivity, ILogger<SendEmailContactWorkflow> logger)
    {
        _processTryHangFireActivity = processTryHangFireActivity;
        _logger = logger;
    }
    public async Task SendEmailAsync(Guid contactId)
    {
        _logger.LogInformation("🚀 Starting SendEmailContactWorkflow...");
        await _processTryHangFireActivity.ExecuteAsync(contactId);
        _logger.LogInformation("✅ Finished SendEmailContactWorkflow.");
    }
}