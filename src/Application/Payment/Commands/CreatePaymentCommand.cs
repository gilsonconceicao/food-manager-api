using Api.Services;
using Application.Common.Exceptions;
using Domain.Interfaces;
using Infrastructure.Database;
using MediatR;
using MercadoPago.Client.Preference;
using Microsoft.EntityFrameworkCore;

namespace Application.Payment.Commands;

public class CreatePaymentCommand : IRequest<string>
{
    public List<Guid> Items { get; set; }

}

public class CreatePaymentCommandHandler : IRequestHandler<CreatePaymentCommand, string>
{
    private readonly IHttpUserService _httpUserService;
    private readonly DataBaseContext _context;
    private readonly IPaymentCommunication _paymentCommunication;

    public CreatePaymentCommandHandler(
        IHttpUserService httpUserService,
        DataBaseContext context,
        IPaymentCommunication paymentCommunication
    )
    {
        _context = context;
        _httpUserService = httpUserService;
        _paymentCommunication = paymentCommunication;
    }

    public async Task<string> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
    {

        var userAuthenticated = await _httpUserService.GetAuthenticatedUser();
        
        var user = _context.Users.FirstOrDefault(x => x.CreatedByUserId == userAuthenticated.UserId)
            ?? throw new NotFoundException("Usuário não encontrado.");

        var cartItems = _context.Carts
            .Include(ci => ci.Food)
            .Where(ci => request.Items.Contains(ci.Id) && ci.CreatedByUserId == userAuthenticated.UserId)
            .ToList();

        if (!cartItems.Any())
            throw new NotFoundException("Nenhum item encontrado no carrinho para os IDs fornecidos.");

        var items = cartItems.Select(ci => new PreferenceItemRequest
        {
            Id = ci.Id.ToString(),
            Title = ci.Food.Name,
            Description = ci.Food.Description,
            PictureUrl = ci.Food.UrlImage,
            CategoryId = ci.Food.Category.ToString(),
            Quantity = ci.Quantity <= 0 ? 1 : ci.Quantity,
            UnitPrice = ci.Food.Price / 100,
            CurrencyId = "BRL",
            Warranty = false,
            EventDate = DateTime.UtcNow
        }).ToList();

        var preference = await _paymentCommunication.CreateCheckoutProAsync(items);
        return preference.InitPoint;
    }
}
