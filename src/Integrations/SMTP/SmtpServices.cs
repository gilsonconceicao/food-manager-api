using System.Net;
using System.Net.Mail;

namespace Integrations.SMTP;

public interface ISmtpService
{
    Task SendEmailAsync(string from, string to, string subject, string body);
}

public class SmtpServices : ISmtpService
{
    public async Task SendEmailAsync(string from, string to, string subject, string body)
    {
        try
        {
            var startSmptClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("", ""),
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