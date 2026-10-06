using MessagingServiceModel;

namespace MessagingServiceBL.Interface
{
    public interface IEmailBL
    {
        void SendEmail(EmailRequest request);
    }
}