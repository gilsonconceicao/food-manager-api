
using Api.Enums;
using Api.Services;
using Application.Common.Exceptions;
using Domain.Models;
using Infrastructure.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Carts.Commands;

public class CartUpdateCommand : IRequest<bool>
{
    public Guid CartId { get; set; }
    public Guid ItemId { get; set; }
    public int? Quantity { get; set; }
}

public class CartUpdateCommandHandler : IRequestHandler<CartUpdateCommand, bool>
{
    private readonly DataBaseContext _context;
    private readonly ICurrentUser _httpUserService;

    public CartUpdateCommandHandler(
        DataBaseContext context,
        ICurrentUser httpUserService
    )
    {
        _context = context;
        _httpUserService = httpUserService;
    }

    public async Task<bool> Handle(CartUpdateCommand request, CancellationToken cancellationToken)
    {
        var user = await _httpUserService.GetAuthenticatedUser();

        Cart cart = await _context.Carts
            .FirstOrDefaultAsync(c => c.Id == request.CartId && user.UserId == c.CreatedByUserId)
            ?? throw new NotFoundException("Comida não encontrada ou não existe.");

        if (request.Quantity != null)
            cart.Quantity = request.Quantity;

        cart.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return true;
    }
}