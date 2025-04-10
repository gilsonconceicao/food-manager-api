
using Api.Services;
using AutoMapper;
using Domain.Models;
using FirebaseAdmin.Auth;
using Infrastructure.Database;
using Integrations.Settings;
using Integrations.SMTP;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
#nullable disable
namespace Application.Carts.Commands;

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
    public MergeUsersFirebaseCommandHandler(
        DataBaseContext context,
        ICurrentUser httpUserService,
        IMapper mapper,
        ISmtpService smtpService,
        IOptions<SmtpServicesSettings> smtpSettins
    )
    {
        _context = context;
        _httpUserService = httpUserService;
        _mapper = mapper;
        _smtpService = smtpService;
        _smtpServicesSetting = smtpSettins.Value;
    }

    public async Task<bool> Handle(MergeUsersFirebaseCommand request, CancellationToken cancellationToken)
    {
        var exportedUsers = await _httpUserService.GetExportedUserRecords();
        var currentUsers = await _context.Users.ToListAsync();
        var usersMapped = new List<User>();

        foreach (var user in exportedUsers)
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

        await _smtpService.SendEmailAsync(
            from: _smtpServicesSetting.NetworkCredentialUserName,
            to: "gilsonsantosjunior02@gmail.com",
            subject: "📊 Relatório Diário - Usuários sincronizados",
            body: EmailTemplates.DailyReportMergeUsersHtml(DateTime.Now, usersMapped)
        );

        return true;
    }
}