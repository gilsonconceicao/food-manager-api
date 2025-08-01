using Domain.Enums;
using Api.Services;
using Domain.Common.Exceptions;
using Domain.Models;
using Infrastructure.Database;
using Integrations.MercadoPago;
using MediatR;
using MercadoPago.Resource.Payment;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
#nullable disable

public class GetPaymentByIdQuery : IRequest<Pay>
{
    public long PaymentId { get; set; }

}

public class GetPaymentByIdQueryHandler : IRequestHandler<GetPaymentByIdQuery, Pay>
{
    private readonly ICurrentUser _httpUserService;
    private readonly DataBaseContext _context;
        private readonly IMercadoPagoClient _mercadoPagoClient;


    public GetPaymentByIdQueryHandler(
        ICurrentUser httpUserService,
        DataBaseContext context,
        IMercadoPagoClient mercadoPagoClient
    )
    {
        _context = context;
        _httpUserService = httpUserService;
        _mercadoPagoClient = mercadoPagoClient;
    }

    public async Task<Pay> Handle(GetPaymentByIdQuery request, CancellationToken cancellationToken)
    {
        var userAuthenticated = await _httpUserService.GetAuthenticatedUser();

        if (request.PaymentId == null || userAuthenticated == null)
            throw new HttpResponseException(
                StatusCodes.Status400BadRequest,
                CodeErrorEnum.INVALID_BUSINESS_RULE.ToString(),
                $"Informe um ID do pagamento válido ou usuário não autenticado"
            );

        var data = await _context.Pays.FirstOrDefaultAsync(p => p.Id == request.PaymentId.ToString());
        return data;
    }
}
