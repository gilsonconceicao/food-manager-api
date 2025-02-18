using Api.Enums;
using Api.Services;
using Application.Carts.Commands.Factories;
using Application.Common.Exceptions;
using Domain.Models;
using Infrastructure.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Carts.Commands;
#nullable disable 
public class CartCreateCommand : IRequest<bool>
{
    public Guid ItemId { get; set; }
    public int? Quantity { get; set; } = 1; 
}

public class CartCreateCommandHandler : IRequestHandler<CartCreateCommand, bool>
{
    private readonly DataBaseContext _context;
    private readonly ICartFactory _CartFactory;
    private readonly ICurrentUser _httpUserService;

    public CartCreateCommandHandler(
        DataBaseContext context,
        ICartFactory cartFactory,
        ICurrentUser httpUserService
    )
    {
        _context = context;
        _CartFactory = cartFactory;
        _httpUserService = httpUserService;
    }

    public async Task<bool> Handle(CartCreateCommand request, CancellationToken cancellationToken)
    {
        var user = await _httpUserService.GetAuthenticatedUser();

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


        var getExistsItem = await _context.Carts
            .FirstOrDefaultAsync(x => 
                request.ItemId == x.FoodId && x.CreatedByUserId == user.UserId);
        
        if (getExistsItem is not null) 
        {
            getExistsItem.Quantity = request.Quantity;
            _context.Carts.Update(getExistsItem);
        } 
        else 
        {
            var newCart = _CartFactory.CreateCart(request.ItemId, request.Quantity);
            _context.Carts.Add(newCart);
        }

        await _context.SaveChangesAsync();
        return true;

    }
}