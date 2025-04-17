using Application.Carts.Commands;
using Domain.Enums;
using Infrastructure.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Workflows.Workflows;

public interface IProcessOrderExpirationActivity
{
    Task ExecuteAsync();
}

public class ProcessOrderExpirationActivity : IProcessOrderExpirationActivity
{
    private readonly DataBaseContext _context;
    private readonly ILogger<OrderExpirationWorkflow> _logger;

    public ProcessOrderExpirationActivity(DataBaseContext context, ILogger<OrderExpirationWorkflow> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task ExecuteAsync()
    {
        var now = DateTime.UtcNow;

        var expiredOrders = await _context.Orders
            .Where(o => o.Status == OrderStatus.AwaitingPayment && o.ExpirationDateTo != null && o.ExpirationDateTo < now)
            .ToListAsync();

        if (expiredOrders.Count == 0)
        {
            _logger.LogWarning("no order with expiration date.");
            return;
        }

        foreach (var order in expiredOrders)
        {
            order.Status = OrderStatus.Expired;
        }

        await _context.SaveChangesAsync();
    }
}
