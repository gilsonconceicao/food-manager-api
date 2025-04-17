using Api.Services;
using Api.Workflows.JobSchedulerService;
using Application.Common.Exceptions;
using Domain.Enums;
using Domain.Interfaces;
using Infrastructure.Database;
using MediatR;
using MercadoPago.Client.Preference;
using Microsoft.EntityFrameworkCore;
#nullable disable
namespace Application.Payment.Commands;

public class CreatePaymentCommand : IRequest<string>
{
    public List<Guid> OrderIds { get; set; }

}

public class CreatePaymentCommandHandler : IRequestHandler<CreatePaymentCommand, string>
{
    private readonly ICurrentUser _httpUserService;
    private readonly DataBaseContext _context;
    private readonly IPaymentCommunication _paymentCommunication;
    private readonly IJobSchedulerService _jobSchedulerService;

    public CreatePaymentCommandHandler(
        ICurrentUser httpUserService,
        DataBaseContext context,
        IPaymentCommunication paymentCommunication,
        IJobSchedulerService jobSchedulerService
    )
    {
        _context = context;
        _httpUserService = httpUserService;
        _paymentCommunication = paymentCommunication;
        _jobSchedulerService = jobSchedulerService;
    }

    public async Task<string> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
    {
        var userAuthenticated = await _httpUserService.GetAuthenticatedUser();
        var userId = userAuthenticated.UserId;

        var orders = await _context.Orders
            .Include(o => o.Items)
                .ThenInclude(i => i.Food)
            .Where(o =>
                request.OrderIds.Contains(o.Id) &&
                o.CreatedByUserId == userId &&
                o.Status == OrderStatus.AwaitingPayment || o.Status == OrderStatus.Expired
            )
            .ToListAsync(cancellationToken);

        if (!orders.Any())
            throw new NotFoundException("Nenhum pedido aguardando aprovação encontrado.");

        var allItems = orders
            .SelectMany(or => or.Items)
            .Select(item => new PreferenceItemRequest
            {
                Id = $"{item.OrderId}-{item.FoodId}",
                Title = $"Pedido #{item.Order.RequestNumber} - {item.Food.Name}",
                Description = $"Item do pedido - {item.Food.Description}",
                PictureUrl = item.Food.UrlImage,
                CategoryId = item.Food.Category.ToString(),
                Quantity = item.Quantity <= 0 ? 1 : item.Quantity,
                UnitPrice = item.Food.Price / 100m,
                CurrencyId = "BRL"
            })
            .ToList();

        var preference = await _paymentCommunication.CreateCheckoutProAsync(allItems);

        foreach (var order in orders)
        {
            order.ExternalPaymentId = preference.ExternalReference;
            order.ExpirationDateTo = DateTime.UtcNow.AddHours(1);
        }

        await _context.SaveChangesAsync(cancellationToken);
        return preference.InitPoint;
    }
}
