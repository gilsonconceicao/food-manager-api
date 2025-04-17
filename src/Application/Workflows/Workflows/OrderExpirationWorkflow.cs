using Microsoft.Extensions.Logging;

namespace Application.Workflows.Workflows;

public class OrderExpirationWorkflow
{
    private readonly IProcessOrderExpirationActivity _processOrderExpirationActivity;
    private readonly ILogger<OrderExpirationWorkflow> _logger;

    public OrderExpirationWorkflow(IProcessOrderExpirationActivity processOrderExpirationActivity, ILogger<OrderExpirationWorkflow> logger)
    {
        _processOrderExpirationActivity = processOrderExpirationActivity;
        _logger = logger;
    }

    public async Task CheckExpiredOrders()
    {
        _logger.LogWarning("Workflow started to processOrderExpirationActivity");
        await _processOrderExpirationActivity.ExecuteAsync();
    }
}