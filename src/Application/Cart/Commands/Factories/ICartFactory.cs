using Domain.Models;

namespace Application.Carts.Commands.Factories; 
public interface ICartFactory
{
    Cart CreateCart(Guid itemId, int? quantity, string? observations);
}
