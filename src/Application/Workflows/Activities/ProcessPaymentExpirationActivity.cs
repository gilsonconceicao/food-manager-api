// Activity
using Domain.Enums;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Workflows.Workflows;

public interface IProcessPaymentExpirationActivity
{
    Task ExecuteAsync();
}

public class ProcessPaymentExpirationActivity : IProcessPaymentExpirationActivity
{
    private readonly DataBaseContext _context;
    private readonly ILogger<ProcessPaymentExpirationActivity> _logger;

    public ProcessPaymentExpirationActivity(
        DataBaseContext context,
        ILogger<ProcessPaymentExpirationActivity> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task ExecuteAsync()
    {
        var now = DateTime.UtcNow;

        var expiredOrders = await _context.Orders
            .Include(o => o.Pay)
            .Where(o =>
                o.Status == OrderStatus.AwaitingPayment &&
                o.Pay != null &&
                o.Pay.ExpirationDateTo != null &&
                o.Pay.ExpirationDateTo < now)
            .ToListAsync();

        if (!expiredOrders.Any())
        {
            _logger.LogInformation("ℹ️ No orders found with expired payments.");
            return;
        }

        foreach (var order in expiredOrders)
        {
            order.Status = OrderStatus.Expired;
            _logger.LogWarning("⚠️ Order {OrderId} marked as Expired (Expired at {ExpirationDate})",
                order.Id, order.Pay.ExpirationDateTo);
        }

        await _context.SaveChangesAsync();

        _logger.LogInformation("✅ {Count} orders marked as Expired.", expiredOrders.Count);
    }
}
