using Domain.Common.Exceptions;
using Domain.Models;
using Infrastructure.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Carts.Commands;

public class CartDeleteCommand : IRequest<bool>
{
    public Guid CartId { get; set; }
}

public class CartDeleteCommandHandler : IRequestHandler<CartDeleteCommand, bool>
{
    private readonly DataBaseContext _context;

    public CartDeleteCommandHandler(
        DataBaseContext context
    )
    {
        _context = context;
    }

    public async Task<bool> Handle(CartDeleteCommand request, CancellationToken cancellationToken)
    {
        Cart cart = await _context.Carts
            .FirstOrDefaultAsync(c => c.Id == request.CartId)
            ?? throw new NotFoundException("Item no carrinho  não encontrada ou não existe.");
            
        _context.Remove(cart);
        await _context.SaveChangesAsync();
        return true;
    }
}