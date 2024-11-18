
using Api.Enums;
using Api.Services;
using Application.Common.Exceptions;
using Domain.Models;
using Infrastructure.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Carts.Commands;

public class CreateUpdateCommand : IRequest<bool>
{
    public Guid CartId { get; set; }
    public Guid ItemId { get; set; }
    public int? Quantity { get; set; }
}

public class CreateUpdateCommandHandler : IRequestHandler<CreateUpdateCommand, bool>
{
    private readonly DataBaseContext _context;
    private readonly IHttpUserService _httpUserService;

    public CreateUpdateCommandHandler(
        DataBaseContext context,
        IHttpUserService httpUserService
    )
    {
        _context = context;
        _httpUserService = httpUserService;
    }

    public async Task<bool> Handle(CreateUpdateCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var user = await _httpUserService.getAuthenticatedUser();

            Cart cart = await _context.Carts
                .FirstOrDefaultAsync(c => c.Id == request.CartId && user.UserId == c.CreatedByUserId)
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