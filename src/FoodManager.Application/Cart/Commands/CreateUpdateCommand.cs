
using FoodManager.API.Enums;
using FoodManager.Application.Common.Exceptions;
using FoodManager.Domain.Models;
using FoodManager.Infrastructure.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FoodManager.Application.Carts.Commands;

public class CreateUpdateCommand : IRequest<bool>
{
    public Guid CartId { get; set; }
    public Guid ItemId { get; set; }
    public int? Quantity { get; set; }
}

public class CreateUpdateCommandHandler : IRequestHandler<CreateUpdateCommand, bool>
{
    private readonly DataBaseContext _context;

    public CreateUpdateCommandHandler(
        DataBaseContext context
    )
    {
        _context = context;
    }

    public async Task<bool> Handle(CreateUpdateCommand request, CancellationToken cancellationToken)
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


            if (request.Quantity != null)
                cart.Quantity = request.Quantity;
                
            cart.UpdatedAt = DateTime.UtcNow;
                
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