
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
using Microsoft.IdentityModel.Tokens;
#nullable disable
namespace Application.Users.Commands;

public class MergeUsersFirebaseCommand : IRequest<bool>
{

}

public class MergeUsersFirebaseCommandHandler : IRequestHandler<MergeUsersFirebaseCommand, bool>
{
    private readonly DataBaseContext _context;
    private readonly ISmtpService _smtpService;
    private readonly ICurrentUser _httpUserService;
    private readonly IMapper _mapper;
    private readonly SmtpServicesSettings _smtpServicesSetting;
    private readonly IConfiguration _config;

    public MergeUsersFirebaseCommandHandler(
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

    public async Task<bool> Handle(MergeUsersFirebaseCommand request, CancellationToken cancellationToken)
    {
        var firebaeUsers = await _httpUserService.GetExportedUserRecords();
        var currentUsers = await _context.Users
            .Include(u => u.Orders)
            .ToListAsync();

        var usersMapped = new List<User>();
        var envDisplay = _config.GetValue<string>("EnvDisplay");

        foreach (var user in firebaeUsers)
        {
            var userExists = currentUsers.FirstOrDefault(x => x.FirebaseUserId == user.Uid);
            if (userExists == null)
            {
                usersMapped.Add(
                    _mapper.Map<ExportedUserRecord, User>(user)
                );
            }
        }

        await _context.Users.AddRangeAsync(usersMapped);
        await _context.SaveChangesAsync();

        var rootUsers = currentUsers.Where(u => u.IsRoot == true).ToList();
        foreach (var admin in rootUsers)
        {
            await _smtpService.SendEmailAsync(
               from: _smtpServicesSetting.NetworkCredentialUserName,
               to: admin.Email,
               subject: "📊 Relatório Diário - Usuários sincronizados",
               body: EmailTemplates.DailyReportMergeUsersHtml(admin, DateTime.Now, usersMapped, envDisplay)
           );
        }

        return true;
    }
}