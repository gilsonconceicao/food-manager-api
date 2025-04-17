using Domain.Enums;
using Infrastructure.Database;

namespace Application.Workflows.Activities; 

public interface IUpdateOrderStatusActivity
{
    Task SetStatusAsync(Guid orderId, OrderStatus newStatus);
}

public class UpdateOrderStatusActivity : IUpdateOrderStatusActivity
{
    private readonly DataBaseContext _context;

    public UpdateOrderStatusActivity(DataBaseContext context)
    {
        _context = context;
    }

    public async Task SetStatusAsync(Guid orderId, OrderStatus newStatus)
    {
        var order = await _context.Orders.FindAsync(orderId);
        if (order == null) return;

        order.Status = newStatus;
        await _context.SaveChangesAsync();
    }
}