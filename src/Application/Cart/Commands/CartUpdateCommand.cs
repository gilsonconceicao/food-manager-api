
using Api.Services;
using Domain.Common.Exceptions;
using Domain.Models;
using Infrastructure.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Carts.Commands;

public class CartUpdateCommand : IRequest<bool>
{
    public Guid OrderId { get; set; }
    public Guid FoodId { get; set; }
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

        Order order = await _context.Orders
            .Include(x => x.Items)
            .ThenInclude(x => x.Food)
            .FirstOrDefaultAsync(c => c.Id == request.OrderId)
            ?? throw new NotFoundException("Pedido não encontrado ou não existe.");

        var item = order.Items.FirstOrDefault(x => x.FoodId == request.FoodId); 
        
        if (request.Quantity != null)
            item.Quantity = request.Quantity;

        order.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return true;
    }
}