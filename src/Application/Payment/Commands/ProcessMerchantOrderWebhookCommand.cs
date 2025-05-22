using Application.Workflows.Workflows;
using Domain.Enums;
using Domain.Interfaces;
using Hangfire;
using MediatR;

namespace Application.Payment.Commands;

public class ProcessMerchantOrderWebhookCommand : IRequest<Unit>
{
    public required string PaymentId { get; set; }
}

public class ProcessMerchantOrderWebhookCommandHandler : IRequestHandler<ProcessMerchantOrderWebhookCommand, Unit>
{
    private readonly IPaymentCommunication _paymentCommunication;
    public ProcessMerchantOrderWebhookCommandHandler(IPaymentCommunication paymentCommunication)
    {
        _paymentCommunication = paymentCommunication;
    }

    public async Task<Unit> Handle(ProcessMerchantOrderWebhookCommand request, CancellationToken cancellationToken)
    {
        await _paymentCommunication.ProcessPaymentWebhookAsync(request.PaymentId);
        return Unit.Value;
    }
}
