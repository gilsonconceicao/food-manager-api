using Domain.Interfaces.Workflow;
using Domain.Interfaces.Workflow.Activities;
namespace Api.Workflows.Workflows; 

public class CartWorkflowJob : ICartWorkflowJob
{
    private readonly ILogger<CartWorkflowJob> _logger;
    private readonly ICartActivity _cartActivity;

    public CartWorkflowJob(ILogger<CartWorkflowJob> logger, ICartActivity cartActivity)
    {
        _logger = logger;
        _cartActivity = cartActivity;
    }

    public async Task ValidationQuantityJob()
    {
        await _cartActivity.ValidationQuantityActivity();
        _logger.LogInformation("Cart validation job started.");
    }
}