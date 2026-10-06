using BusinessLayer.Interface;
using ModelLayer;
using ServiceLayer.Interface;

namespace BusinessLayer.Service
{
    public class UserBl : IUserBl
    {
        private readonly IUserRL _userRL;
        private readonly MessagingService _messagingService;
        public async Task<bool> SaveResetTokenAsync(
    string email,
    string token,
    DateTime expiry)
        {
            return await _userRL.SaveResetTokenAsync(
                email,
                token,
                expiry);
        }
        public UserBl(
            IUserRL userRL,
            MessagingService messagingService)
        {
            _userRL = userRL;
            _messagingService = messagingService;
        }

        public RegistrationModel RegisterUserBL(
            RegistrationModel registrationModel)
        {
            return _userRL.RegisterUserRL(registrationModel);
        }

        public LoginModel LoginUserBL(
            LoginModel loginModel)
        {
            return _userRL.LoginUserRL(loginModel);
        }

        public async Task<bool> ForgotPasswordAsync(ForgotPasswordModel model)
        {
            var user = await _userRL.GetUserByEmailAsync(model.Email);

            if (user == null)
            {
                return false;
            }

            string token = Guid.NewGuid().ToString();

            DateTime expiry = DateTime.UtcNow.AddMinutes(15);

            await _userRL.SaveResetTokenAsync(
                model.Email,
                token,
                expiry);


            await _messagingService.SendEmailAsync(
                user.email,
                "Password Reset",
                $"Your password reset token is: {token}\n\nThis token will expire in 15 minutes."
            );

            return true;
        }

        public async Task<bool> ResetPasswordAsync(
    string email,
    string token,
    string newPassword)
        {
            return await _userRL.ResetPasswordAsync(
                email,
                token,
                newPassword);
        }
    }
}