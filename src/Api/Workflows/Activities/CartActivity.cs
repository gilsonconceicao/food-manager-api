using Infrastructure.Database;
using Integrations.SMTP;
using Microsoft.EntityFrameworkCore;

namespace Domain.Interfaces.Workflow.Activities;

public class CartActivity : ICartActivity
{
    private readonly DataBaseContext _db;
    private readonly ILogger<CartActivity> _logger;
    private readonly ISmtpService _smtpService;


    public CartActivity(DataBaseContext db, ILogger<CartActivity> logger, ISmtpService smtpService)
    {
        _db = db;
        _logger = logger;
        _smtpService = smtpService;
    }

    public async Task ValidationQuantityActivity()
    {
        var carts = await _db.Carts
            .ToListAsync();

        foreach (var cart in carts)
        {
            _logger.LogWarning($"Cart with ID {cart.Id} has invalid quantity: {cart.Quantity}.");
            cart.Quantity = cart.Quantity + 1;
            _db.Carts.Update(cart);
        }

        await _db.SaveChangesAsync();
        _logger.LogInformation("Completed! Quantity the carts updated.");
    }

}