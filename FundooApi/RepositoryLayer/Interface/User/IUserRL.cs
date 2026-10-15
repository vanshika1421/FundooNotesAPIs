using ModelLayer.User.RequestModel;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.Interface.User
{
    public interface IUserRL
    {
        Task<bool> UpdatePasswordAsync(string email,string newPassword);
        RegistrationModel RegisterUserRL(RegistrationModel registrationModel);
        LoginModel LoginUserRL(LoginModel loginModel);
        Task<(LoginModel user, int userId, string userName)> GetLoginUserWithIdAsync(string email, string password);
        Task<UserEntity?> GetUserByEmailAsync(string email);
      
    }
}
