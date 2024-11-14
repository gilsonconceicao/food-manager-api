using FoodManager.Domain.Models;

namespace FoodManager.Application.Carts.Commands.Factories; 
public interface ICartFactory
{
    Cart CreateCart(string UserId, Guid itemId, int? quantity);
}
