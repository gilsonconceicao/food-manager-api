using Api.Enums;
using Application.Carts.Commands.Factories;
using Application.Common.Exceptions;
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
        try
        {
            Cart cart = await _context.Carts
                .FirstOrDefaultAsync(c => c.Id == request.CartId)
                ?? throw new HttpResponseException
                {
                    Status = 404,
                    Value = new
                    {
                        Code = CodeErrorEnum.NOT_FOUND_RESOURCE.ToString(),
                        Message = "Item no carrinho  não encontrada ou não existe",
                    }
                };

            cart.IsDeleted = true;
            await _context.SaveChangesAsync();
            return true;
        }
        catch (HttpResponseException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.ToString());
        }
    }
}