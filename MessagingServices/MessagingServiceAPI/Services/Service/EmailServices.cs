using MessagingServiceAPI.Services.Interface;
using System.Net;
using System.Net.Mail;

namespace MessagingServiceAPI.Services.Service
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void SendEmail(string toEmail, string subject, string body)
        {
            var email = _configuration["EmailSettings:Email"];
            var password = _configuration["EmailSettings:Password"];
            var host = _configuration["EmailSettings:Host"];
            var port = int.Parse(_configuration["EmailSettings:Port"]!);

            MailMessage mail = new MailMessage();

            mail.From = new MailAddress(email!);
            mail.To.Add(toEmail);
            mail.Subject = subject;
            mail.Body = body;

            SmtpClient smtp = new SmtpClient(host, port);

            smtp.Credentials = new NetworkCredential(email, password);
            smtp.EnableSsl = true;

            smtp.Send(mail);
        }
    }
}