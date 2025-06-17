using Domain.Enums;
using Api.Services;
using Api.Workflows.JobSchedulerService;
using Domain.Common.Exceptions;
using Application.Workflows.Workflows;
using Domain.Enums;
using Domain.Models;
using Domain.Models.Request;
using Infrastructure.Database;
using Integrations.Interfaces;
using MediatR;
using MercadoPago.Client.Payment;
using MercadoPago.Resource.Payment;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

#nullable enable
public class CreatePaymentCommand : IRequest<Payment>
{
    public Guid OrderId { get; set; }
    public PaymentMethodEnum PaymentMethod { get; set; }
    public int Installments { get; set; } = 1;
    public CardDataRequestDto? Card { get; set; }
}

public class CreatePaymentCommandHandler : IRequestHandler<CreatePaymentCommand, Payment>
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

    public async Task<Payment> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
    {
        var userAuthenticated = await _httpUserService.GetAuthenticatedUser();
        var userId = userAuthenticated.UserId;

        var order = await _context.Orders
            .Include(o => o.Items)
                .ThenInclude(i => i.Food)
            .FirstOrDefaultAsync(o =>
                o.CreatedByUserId == userId &&
                (o.Status == OrderStatus.AwaitingPayment || o.Status == OrderStatus.Expired) &&
                o.Id == request.OrderId, cancellationToken);


        if (order == null)
            throw new NotFoundException("Nenhum pedido aguardando aprovação encontrado.");

        var totalValue = order.Items.Sum(i => i.Quantity * i.Food.Price);

        if (totalValue == null || totalValue == 0)
            throw new NotFoundException("Valor do pedido inválido.");

        var description = $"Pedido #{order.RequestNumber}: " +
                          string.Join(", ", order.Items.Select(i => $"{i.Food.Name} ({i.Quantity}x)"));

        var amount = (Int32)order.TotalValue / 100m;

        try
        {
            var paymentRequest = await _paymentCommunication.CreatePaymentAsync(
                request.PaymentMethod,
                amount,
                description,
                request.Installments,
                request.Card
            );

            var client = new PaymentClient();
            var payment = await client.CreateAsync(paymentRequest);

            order.PaymentId = payment.Id.ToString()!;
            order.Status = OrderStatus.AwaitingPayment;

            await _context.SaveChangesAsync(cancellationToken);

            var pay = new Pay
            {
                OrderId = order.Id,
                Id = payment.Id.ToString()!,
                Description = payment.Description,
                Status = payment.Status,
                PaymentTypeId = payment.PaymentTypeId,
                PaymentMethodId = payment.PaymentMethodId,
                CurrencyId = payment.CurrencyId,
                Installments = payment.Installments ?? 1,
                TransactionAmount = payment.TransactionAmount ?? 0,
                ExternalReference = payment.ExternalReference,
                NotificationUrl = payment.NotificationUrl,
                DateCreated = (payment.DateCreated ?? DateTime.UtcNow).ToUniversalTime(),
                DateLastUpdated = (payment.DateLastUpdated ?? DateTime.UtcNow).ToUniversalTime(),
                ExpirationDateTo = (payment.DateOfExpiration ?? DateTime.UtcNow.AddMinutes(10)).ToUniversalTime(),
                QrCode = payment.PointOfInteraction?.TransactionData?.QrCode,
                QrCodeBase64 = payment.PointOfInteraction?.TransactionData?.QrCodeBase64,
                CollectorId = (long)payment.CollectorId!,
                IssuerId = payment.IssuerId,
            };

            var existingPay = await _context.Pays
               .FirstOrDefaultAsync(p => p.OrderId == order.Id, cancellationToken);

            if (existingPay is not null)
            {
                _context.Pays.Remove(existingPay);
            }

            await _context.Pays.AddAsync(pay, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            _jobSchedulerService.Schedule<PaymentExpirationWorkflow>(
                job => job.CheckExpiredOrders(),
                TimeSpan.FromMinutes(10)
            );

            return payment;
        }
        catch (Exception ex)
        {
            throw new HttpResponseException(
                StatusCodes.Status400BadRequest,
                CodeErrorEnum.PAYMENT_FAILURE.ToString(),
                ex.Message.ToString()
            );
        }
    }
}