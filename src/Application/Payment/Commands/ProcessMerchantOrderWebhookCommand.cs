using Application.Workflows.Workflows;
using Domain.Enums;
using Domain.Interfaces;
using Hangfire;
using MediatR;

namespace Application.Payment.Commands;

public class ProcessMerchantOrderWebhookCommand : IRequest<Unit>
{
    public string PaymentId { get; set; }
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
        string paymentId = request.PaymentId;

        var result = await _paymentCommunication.ProcessPaymentWebhookAsync(paymentId);

        if (result.Order != null && result.Success)
        {
            BackgroundJob.Schedule<UpdateOrderStatusWorkflow>(
               activity => activity.UpdateStatusAsync(result.Order.Id, OrderStatus.InPreparation),
               TimeSpan.FromHours(1)
           );
        }
        return await Task.FromResult(Unit.Value);
    }
}
