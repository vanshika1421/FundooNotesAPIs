using ModelLayer;
using RepositoryLayer.Entity;

namespace BusinessLayer.Interface
{
    public interface IUserBl
    {
        RegistrationModel RegisterUserBL(
            RegistrationModel registrationModel);

        LoginModel LoginUserBL(
            LoginModel loginModel);

        Task<bool> ForgotPasswordAsync(
            ForgotPasswordModel model);
        Task<bool> SaveResetTokenAsync(
    string email,
    string token,
    DateTime expiry);
        Task<bool> ResetPasswordAsync(
 
    string token,
    string newPassword);
    }
}