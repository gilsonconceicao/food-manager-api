using Application.Workflows.Activities;
using Domain.Enums;
using Microsoft.Extensions.Logging;

namespace Application.Workflows.Workflows;

public class UpdateOrderStatusWorkflow
{
    private readonly IUpdateOrderStatusActivity _updateOrderStatusActivity;
    private readonly ILogger<UpdateOrderStatusWorkflow> _logger;

    public UpdateOrderStatusWorkflow(IUpdateOrderStatusActivity updateOrderStatusActivity, ILogger<UpdateOrderStatusWorkflow> logger)
    {
        _updateOrderStatusActivity = updateOrderStatusActivity;
        _logger = logger;
    }

    public async Task UpdateStatusAsync(Guid orderId, OrderStatus newStatus)
    {
        _logger.LogWarning("Workflow started to UpdateOrderStatusWorkflow.UpdateStatusAsync");
        await _updateOrderStatusActivity.SetStatusAsync(orderId, newStatus);
    }
}