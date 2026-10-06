using MessagingServiceModel;
using MessagingServiceRL.Interface;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;

namespace MessagingServiceRL.Services
{
    public class EmailRL : IEmailRL
    {
        private readonly IConfiguration _configuration;

        public EmailRL(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void SendEmail(EmailRequest request)
        {
            var email = _configuration["EmailSettings:Email"];
            var password = _configuration["EmailSettings:Password"];
            var host = _configuration["EmailSettings:Host"];
            var port = int.Parse(_configuration["EmailSettings:Port"]!);

            MailMessage mail = new MailMessage();

            mail.From = new MailAddress(email!);
            mail.To.Add(request.ToEmail);
            mail.Subject = request.Subject;
            mail.Body = request.Body;

            SmtpClient smtp = new SmtpClient(host, port);

            smtp.Credentials = new NetworkCredential(
                email,
                password
            );

            smtp.EnableSsl = true;

            smtp.Send(mail);
        }
    }
}