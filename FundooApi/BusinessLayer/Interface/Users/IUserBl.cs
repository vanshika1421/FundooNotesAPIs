using ModelLayer.User.RequestModel;
using RepositoryLayer.Entity;

namespace BusinessLayer.Interface.Users
{
    public interface IUserBl
    {
        RegistrationModel RegisterUserBL(RegistrationModel registrationModel);

        LoginModel LoginUserBL(LoginModel loginModel);

        Task<(LoginModel user, int userId, string userName)> GetLoginUserWithIdAsync(string email, string password);

        Task<bool> ForgotPasswordAsync(ForgotPasswordModel model);
       
        Task<bool> ResetPasswordAsync( string token,string newPassword);

    }
}
