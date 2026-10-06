using MessagingServiceBL.Interface;
using MessagingServiceModel;
using MessagingServiceRL.Interface;

namespace MessagingServiceBL.Services
{
    public class EmailBL : IEmailBL
    {
        private readonly IEmailRL _emailRL;

        public EmailBL(IEmailRL emailRL)
        {
            _emailRL = emailRL;
        }

        public void SendEmail(EmailRequest request)
        {
            _emailRL.SendEmail(request);
        }
    }
}