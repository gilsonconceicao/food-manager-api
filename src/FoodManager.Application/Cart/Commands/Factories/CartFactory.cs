using FoodManager.Domain.Models;

namespace FoodManager.Application.Carts.Commands.Factories;

public class CartFactory : ICartFactory
{
    public Cart CreateCart(Guid itemId, int? quantity, string? resource)
    {
        return new Cart
        {
            Id = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow,
            ItemId = itemId,
            Quantity = quantity,
            Resource = resource
        };
    }
}