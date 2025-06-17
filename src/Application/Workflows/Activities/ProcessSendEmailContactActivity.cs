using Application.Carts.Commands;
using Application.Contacts.Commands;
using Domain.Models;
using Infrastructure.Database;
using Integrations.Settings;
using Integrations.SMTP;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Application.Workflows.Workflows;

public interface IProcessSendEmailContactActivity
{
    Task ExecuteAsync(Guid contactId);
}

public class ProcessSendEmailContactActivity : IProcessSendEmailContactActivity
{
        private readonly ILogger<ProcessSendEmailContactActivity> _logger;
         private readonly ISmtpService _smtpService;
    private readonly SmtpServicesSettings _smtpServicesSetting;
    private readonly DataBaseContext _context;


    public ProcessSendEmailContactActivity(
        ISmtpService smtpService,
        IOptions<SmtpServicesSettings> smtpServicesSetting,
        DataBaseContext context,
        ILogger<ProcessSendEmailContactActivity> logger
    )
    {
        _smtpService = smtpService;
        _smtpServicesSetting = smtpServicesSetting.Value;
        _context = context;
        _logger = logger;
    }

    public async Task ExecuteAsync(Guid contactId)
    {
        var contact = await _context.Contacts.FirstOrDefaultAsync(c => c.Id == contactId);
        if (contact == null)
        {
            _logger.LogWarning("❌ Contact with ID {ContactId} not found.", contactId);
            return;
        }

        await _smtpService.SendEmailAsync(
           from: _smtpServicesSetting.NetworkCredentialUserName,
           to: "gilsonsantosjunior02@gmail.com",
           subject: $"📲 Contato de cliente - ({contact.Email})",
           body: EmailTemplates.SendEmailContactTemplate(contact)
       );

        _logger.LogInformation("✅ Email sent successfully to {Email}", contact.Email);
    }
}
