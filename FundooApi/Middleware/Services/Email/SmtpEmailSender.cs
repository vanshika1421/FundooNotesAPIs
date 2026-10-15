
using Middleware.Services.Email;

using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace Middleware.Services.Email;

public class SmtpEmailSender : IEmailSender
{
    private readonly IConfiguration _configuration;

    public SmtpEmailSender(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendEmailAsync(
        string to,
        string subject,
        string body)
    {
        var smtpHost = _configuration["EmailSettings:Host"]
            ?? throw new InvalidOperationException("SMTP host is missing.");

        int smtpPort = int.Parse(
            _configuration["EmailSettings:Port"]
            ?? throw new InvalidOperationException("SMTP port is missing."));

        string username = _configuration["EmailSettings:Email"]
            ?? throw new InvalidOperationException("Email username is missing.");

        string password = _configuration["EmailSettings:Password"]
            ?? throw new InvalidOperationException("Email password is missing.");

        using var message = new MailMessage
        {
            From = new MailAddress(username),
            Subject = subject,
            Body = body,
            IsBodyHtml = false
        };

        message.To.Add(to);

        using var smtp = new SmtpClient(smtpHost, smtpPort)
        {
            EnableSsl = true,
            Credentials = new NetworkCredential(username, password)
        };

        await smtp.SendMailAsync(message);
    }

}
