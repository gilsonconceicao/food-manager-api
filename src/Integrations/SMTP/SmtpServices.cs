using System.Net;
using System.Net.Mail;
using Integrations.Settings;
using Microsoft.Extensions.Options;

namespace Integrations.SMTP;

public interface ISmtpService
{
    Task SendEmailAsync(string from, string to, string subject, string body);
}

public class SmtpServices : ISmtpService
{
    private readonly SmtpServicesSettings _smtpServicesSetting;
    public SmtpServices(IOptions<SmtpServicesSettings> smtpSettins)
    {
        _smtpServicesSetting = smtpSettins.Value;
    }
    public async Task SendEmailAsync(string from, string to, string subject, string body)
    {
        try
        {
            var startSmptClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(_smtpServicesSetting.NetworkCredentialUserName, _smtpServicesSetting.NetworkCredentialUserPassword),
                EnableSsl = true
            };

            var customMailMessage = new MailMessage
            {
                From = new MailAddress(from),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            customMailMessage.To.Add(to);

            await startSmptClient.SendMailAsync(customMailMessage);
        }
        catch (Exception ex)
        {
            var errorMessage = ex.Message;
            throw new Exception(errorMessage);
        }
    }
}