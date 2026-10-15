
namespace Middleware.Services.Email;

public static class PasswordResetEmailTemplate
{
    public static string Build(string firstName, string token)
    {
        return $"""
            Hello {firstName},

            We received a request to reset the password
            associated with your Fundoo Notes account.

            Your password reset token is:

            {token}

            This token is valid for 15 minutes.

            For your security:
            - Do not share this token with anyone.
            - If you did not request a password reset,
              please ignore this email.

            Regards,
            Fundoo Notes Team

            This is an automated email. Please do not reply.
            """;
    }
}
