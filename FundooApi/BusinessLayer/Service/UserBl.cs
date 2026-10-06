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

        public LoginModel LoginUserBL(LoginModel loginModel)
        {
            return _userRL.LoginUserRL(loginModel);
        }

        public async Task<bool> ForgotPasswordAsync(
            ForgotPasswordModel model)
        {
            // Find user by email
            var user = await _userRL.GetUserByEmailAsync(model.Email);

            if (user == null)
            {
                return false;
            }

            // Generate a unique reset token
            string token = Guid.NewGuid().ToString();

            // Token will expire after 15 minutes
            DateTime expiry = DateTime.UtcNow.AddMinutes(15);

            // Save token and expiry in database
            await _userRL.SaveResetTokenAsync(
                model.Email,
                token,
                expiry);

            // Professional email body
            string emailBody = $@"
Hello {user.FirstName},

We received a request to reset the password associated with your Fundoo Notes account.

Your password reset request has been received successfully. Please use the verification token below to proceed with resetting your password.

Reset Token:
{token}

This token is valid for 15 minutes and will expire after that.

For your security:
- Do not share this token with anyone.
- If you did not request a password reset, please ignore this email.
- Your password will not be changed unless the reset process is completed successfully.

If you have any questions or believe this request was made without your permission, please contact our support team.

Regards,
Fundoo Notes Team

This is an automated email. Please do not reply to this message.
";

            // Send reset email
            await _messagingService.SendEmailAsync(
                user.email,
                "Fundoo Notes - Password Reset Request",
                emailBody);

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