using FoodManager.Domain.Models;

namespace FoodManager.Application.Carts.Commands.Factories;

public class CartFactory : ICartFactory
{
    public Cart CreateCart(string UserId, Guid itemId, int? quantity)
    {
        return new Cart
        {
            Id = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow,
            ItemId = itemId,
            Quantity = quantity,
            UserId = UserId, 
            IsDeleted = false
        };
    }
}