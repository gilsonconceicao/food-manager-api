using Microsoft.Extensions.Logging;

namespace Application.Workflows.Workflows;

public class PaymentExpirationWorkflow
{
    private readonly IProcessPaymentExpirationActivity _processOrderExpirationActivity;
    private readonly ILogger<PaymentExpirationWorkflow> _logger;

    public PaymentExpirationWorkflow(IProcessPaymentExpirationActivity processOrderExpirationActivity, ILogger<PaymentExpirationWorkflow> logger)
    {
        _processOrderExpirationActivity = processOrderExpirationActivity;
        _logger = logger;
    }

    public async Task CheckExpiredOrders()
    {
        _logger.LogInformation("🚀 Starting PaymentExpirationWorkflow...");
        await _processOrderExpirationActivity.ExecuteAsync();
        _logger.LogInformation("✅ Finished PaymentExpirationWorkflow.");
    }
}