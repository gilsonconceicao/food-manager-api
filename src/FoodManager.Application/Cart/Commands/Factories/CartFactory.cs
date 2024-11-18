using FoodManager.Domain.Models;

namespace FoodManager.Application.Carts.Commands.Factories;

public class CartFactory : ICartFactory
{
    public Cart CreateCart(Guid itemId, int? quantity)
    {
        return new Cart
        {
            Id = Guid.NewGuid(),
            ItemId = itemId,
            Quantity = quantity
        };
    }
}