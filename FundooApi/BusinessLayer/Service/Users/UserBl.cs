
using BusinessLayer.Interface.Users;
using ModelLayer.User.RequestModel;
using RepositoryLayer.Interface.User;
using Middleware.Services.Email;
using Middleware.Services.JWT;

using System.Security.Claims;

namespace BusinessLayer.Service.Users;

public class UserBl : IUserBl
{
    private readonly IUserRL _userRL;
    private readonly SmtpEmailSender _emailSender;
    private readonly IJwtTokenServices _jwtTokenServices;

    public UserBl(
        IUserRL userRL,
        SmtpEmailSender emailSender,
        IJwtTokenServices jwtTokenServices)
    {
        _userRL = userRL;
        _emailSender = emailSender;
        _jwtTokenServices = jwtTokenServices;
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

    public async Task<(LoginModel user, int userId, string userName)>
        GetLoginUserWithIdAsync(string email, string password)
    {
        return await _userRL.GetLoginUserWithIdAsync(
            email, password);
    }

    public async Task<bool> ForgotPasswordAsync(
        ForgotPasswordModel model)
    {
        var user = await _userRL.GetUserByEmailAsync(model.Email);

        if (user == null)
        {
            return false;
        }

        // Generate a signed JWT specifically for password reset.
        string resetToken =
            _jwtTokenServices.GenerateToken(user.email , user.Id.ToString() );

        // Send the token through your existing email service.
        string body = PasswordResetEmailTemplate.Build(
            $"{user.FirstName} {user.LastName}",
            resetToken);

        await _emailSender.SendEmailAsync(
            user.email,
            "Fundoo Notes - Password Reset Request",
            body);

        return true;
    }

    public async Task<bool> ResetPasswordAsync(
        string token,
        string newPassword)
    {
        if (string.IsNullOrWhiteSpace(token) ||
            string.IsNullOrWhiteSpace(newPassword))
        {
            return false;
        }

        // Validate the JWT signature, issuer, audience and expiry.
        ClaimsPrincipal? principal =
            _jwtTokenServices.ValidateToken(token);

        if (principal == null)
        {
            return false;
        }

        // Ensure this is a password-reset token,
        // not a normal login JWT.
        string? tokenType =
            principal.FindFirst("token_type")?.Value;

        if (tokenType != "password_reset")
        {
            return false;
        }

        // Extract the user's email from the validated JWT.
        string? email =
            principal.FindFirst(ClaimTypes.Email)?.Value;

        if (string.IsNullOrWhiteSpace(email))
        {
            return false;
        }

        // Update the password; do not store the JWT in the database.
        return await _userRL.UpdatePasswordAsync(
            email,
            newPassword);
    }
}
