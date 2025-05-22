using Api.Enums;
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
    public Guid OrderId { get; set; }
    public PaymentMethodEnum PaymentMethod { get; set; }
    public int Installments { get; set; } = 1;
    public string PaymentMehodToken { get; set; }

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

        var order = await _context.Orders
            .Include(o => o.Items)
                .ThenInclude(i => i.Food)
            .FirstOrDefaultAsync(o =>
                o.CreatedByUserId == userId &&
                (o.Status == OrderStatus.AwaitingPayment || o.Status == OrderStatus.Expired) &&
                o.Id == request.OrderId,
                cancellationToken);

        if (order == null)
            throw new NotFoundException("Nenhum pedido aguardando aprovação encontrado.");
        if (order.TotalValue == null || order.TotalValue == 0)
            throw new NotFoundException("Valor do pedido inválido.");

        var description = $"Pedido #{order.RequestNumber}: " +
                          string.Join(", ", order.Items.Select(i => $"{i.Food.Name} ({i.Quantity}x)"));

        var amount = (Int32)order.TotalValue / 100m;

        var payment = await _paymentCommunication.CreatePaymentAsync(
            request.PaymentMethod,
            amount,
            description,
            request.Installments,
            request.PaymentMehodToken
        );

        order.PaymentId = payment.Id.ToString();
        order.ExpirationDateTo = DateTime.UtcNow.AddHours(1);
        order.NumberOfInstallments = request.Installments;

        await _context.SaveChangesAsync(cancellationToken);
        return payment.Id.ToString();
    }
}