
using Integrations.Settings;
using Integrations.SMTP;
using MediatR;
using Microsoft.Extensions.Options;

#nullable disable
namespace Application.Carts.Commands;

public class TryHangFireCommand : IRequest<bool>
{

}

public class TryHangFireCommandHandler : IRequestHandler<TryHangFireCommand, bool>
{
    private readonly ISmtpService _smtpService;
    private readonly SmtpServicesSettings _smtpServicesSetting;

    public TryHangFireCommandHandler(
        ISmtpService smtpService,
        IOptions<SmtpServicesSettings> smtpSettins
    )
    {
        _smtpService = smtpService;
        _smtpServicesSetting = smtpSettins.Value;
    }

    public async Task<bool> Handle(TryHangFireCommand request, CancellationToken cancellationToken)
    {
        await _smtpService.SendEmailAsync(
           from: _smtpServicesSetting.NetworkCredentialUserName,
           to: "gilsonsantosjunior02@gmail.com",
           subject: "📊 HangFire - Workflow Ativo",
           body: EmailTemplates.TryHangfireTemplete()
       );

        return true;
    }
}