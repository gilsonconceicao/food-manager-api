using Domain.Models;

namespace Application.Carts.Commands.Factories;

public class CartFactory : ICartFactory
{
    public Cart CreateCart(Guid itemId, int? quantity, string? observations)
    {
        return new Cart
        {
            Id = Guid.NewGuid(),
            FoodId = itemId,
            Quantity = quantity,
            Observations = observations
        };
    }
}