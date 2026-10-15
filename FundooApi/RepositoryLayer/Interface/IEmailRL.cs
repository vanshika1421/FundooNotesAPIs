using MessagingServiceModel;

namespace MessagingServiceRL.Interface
{
    public interface IEmailRL
    {
        void SendEmail(EmailRequest request);
    }
}