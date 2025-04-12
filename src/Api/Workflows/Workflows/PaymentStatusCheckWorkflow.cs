using Domain.Interfaces;

namespace Api.Workflows.Workflows;
public class PaymentStatusCheckWorkflow
{
    private readonly IPaymentCommunication _paymentCommunication;

    public PaymentStatusCheckWorkflow(IPaymentCommunication paymentCommunication)
    {
        _paymentCommunication = paymentCommunication;
    }

    public async Task CheckPendingPaymentsAsync()
    {
        await _paymentCommunication.VerifyPendingAsync();
    }
}