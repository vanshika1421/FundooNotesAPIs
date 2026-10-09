using BusinessLayer.Interface;
using ModelLayer;
using ServiceLayer.Interface;
using System.Security.Cryptography;
using System.Text;

namespace BusinessLayer.Service
{
    public class UserBl : IUserBl
    {
        private readonly IUserRL _userRL;
        private readonly MessagingService _messagingService;

        public UserBl(
            IUserRL userRL,
            MessagingService messagingService)
        {
            _userRL = userRL;
            _messagingService = messagingService;
        }

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

            // Generate a cryptographically secure random token
            byte[] tokenBytes = RandomNumberGenerator.GetBytes(32);

            string token = Convert.ToBase64String(tokenBytes)
                .Replace("+", "-")
                .Replace("/", "_")
                .Replace("=", "");

            // Token will expire after 15 minutes
            DateTime expiry = DateTime.UtcNow.AddMinutes(15);

            // Hash the token before storing it in the database
            string hashedToken = HashResetToken(token);

            // Store ONLY the hashed token in the database
            await _userRL.SaveResetTokenAsync(
                model.Email,
                hashedToken,
                expiry);

            // Send the ORIGINAL token to the user's email
            string emailBody = $@"
Hello {user.FirstName} {user.LastName},

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
            string token,
            string newPassword)
        {
            // User sends the ORIGINAL token received by email.
            // Hash it so it can be compared with the hashed token in DB.
            string hashedToken = HashResetToken(token);

            // Email is no longer required.
            // Repository will find the user using the hashed token.
            return await _userRL.ResetPasswordAsync(
                hashedToken,
                newPassword);
        }

        private static string HashResetToken(string token)
        {
            byte[] tokenBytes = Encoding.UTF8.GetBytes(token);
            byte[] hashBytes = SHA256.HashData(tokenBytes);

            return Convert.ToBase64String(hashBytes);
        }
    }
}