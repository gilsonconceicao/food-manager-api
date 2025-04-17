
using Api.Services;
using AutoMapper;
using Domain.Models;
using FirebaseAdmin.Auth;
using Infrastructure.Database;
using Integrations.Settings;
using Integrations.SMTP;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

#nullable disable
namespace Application.Carts.Commands;

public class TryHangFireCommand : IRequest<bool>
{

}

public class TryHangFireCommandHandler : IRequestHandler<TryHangFireCommand, bool>
{
    private readonly DataBaseContext _context;
    private readonly ISmtpService _smtpService;
    private readonly ICurrentUser _httpUserService;
    private readonly IMapper _mapper;
    private readonly SmtpServicesSettings _smtpServicesSetting;
    private readonly IConfiguration _config;

    public TryHangFireCommandHandler(
        DataBaseContext context,
        ICurrentUser httpUserService,
        IMapper mapper,
        ISmtpService smtpService,
        IOptions<SmtpServicesSettings> smtpSettins,
        IConfiguration configuration
    )
    {
        _context = context;
        _httpUserService = httpUserService;
        _mapper = mapper;
        _smtpService = smtpService;
        _smtpServicesSetting = smtpSettins.Value;
        _config = configuration;
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