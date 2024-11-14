using FoodManager.API.Enums;
using FoodManager.Application.Carts.Commands.Factories;
using FoodManager.Application.Common.Exceptions;
using FoodManager.Domain.Models;
using FoodManager.Infrastructure.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FoodManager.Application.Carts.Commands;
#nullable disable 
public class CartCreateCommand : IRequest<bool>
{
    public string UserId { get; set; }
    public Guid ItemId { get; set; }
    public int? Quantity { get; set; }
}

public class CartCreateCommandHandler : IRequestHandler<CartCreateCommand, bool>
{
    private readonly DataBaseContext _context;
    private readonly ICartFactory _CartFactory;

    public CartCreateCommandHandler(
        DataBaseContext context,
        ICartFactory cartFactory
    )
    {
        _context = context;
        _CartFactory = cartFactory;
    }

    public async Task<bool> Handle(CartCreateCommand request, CancellationToken cancellationToken)
    {
        try
        {
            Food existsFoodRelated = await _context.Foods
                    .FirstOrDefaultAsync(c => c.Id == request.ItemId)
                    ?? throw new HttpResponseException
                    {
                        Status = 404,
                        Value = new
                        {
                            Code = CodeErrorEnum.NOT_FOUND_RESOURCE.ToString(),
                            Message = "ID informado não possui nenhum recurso relacionado.",
                        }
                    };

            var newCart = _CartFactory.CreateCart(request.UserId, request.ItemId, request.Quantity);
            _context.Carts.Add(newCart);
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