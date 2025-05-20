using Api.Enums;
using Api.Services;
using Application.Common.Exceptions;
using Infrastructure.Database;
using Integrations.MercadoPago;
using MediatR;
using MercadoPago.Resource.Preference;
#nullable disable
namespace Application.Payment.Commands;

public class GetPreferenceByIdQuery : IRequest<string>
{
    public string PreferenceId { get; set; }

}

public class GetPreferenceByIdQueryHandler : IRequestHandler<GetPreferenceByIdQuery, string>
{
    private readonly ICurrentUser _httpUserService;
    private readonly DataBaseContext _context;
        private readonly IMercadoPagoClient _mercadoPagoClient;


    public GetPreferenceByIdQueryHandler(
        ICurrentUser httpUserService,
        DataBaseContext context,
        IMercadoPagoClient mercadoPagoClient
    )
    {
        _context = context;
        _httpUserService = httpUserService;
        _mercadoPagoClient = mercadoPagoClient;
    }

    public async Task<string> Handle(GetPreferenceByIdQuery request, CancellationToken cancellationToken)
    {
        var userAuthenticated = await _httpUserService.GetAuthenticatedUser();

        if (request.PreferenceId == null || userAuthenticated == null)
            throw new HttpResponseException
            {
                Status = 400,
                Value = new
                {
                    Code = CodeErrorEnum.INVALID_BUSINESS_RULE.ToString(),
                    Message = $"Informe um ID de preferência válido ou usuário não autenticado",
                }
            };

        var data = await _mercadoPagoClient.GetPreferenceByIdAsync(request.PreferenceId);
        return data;
    }
}
