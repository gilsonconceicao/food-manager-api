using Api.Services;
using Domain.Interfaces;
using Infrastructure.Database;
using Integrations.MercadoPago;
using MediatR;

namespace Application.Payment.Commands;

public class CreatePaymentCommand : IRequest<string>
{

}

public class CreatePaymentCommandHandler : IRequestHandler<CreatePaymentCommand, string>
{
    private readonly IHttpUserService _httpUserService;
    private readonly DataBaseContext _context;
    private readonly IPaymentCommunication _PaymentCommunication;

    public CreatePaymentCommandHandler(
        IHttpUserService httpUserService,
        DataBaseContext context,
        IPaymentCommunication paymentCommunication
    )
    {
        _context = context;
        _httpUserService = httpUserService;
        _PaymentCommunication = paymentCommunication;
    }
     
    public Task<string> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
    {
        
        throw new NotImplementedException();
    }
}
