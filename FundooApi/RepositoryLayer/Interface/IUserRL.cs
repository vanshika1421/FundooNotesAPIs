using ModelLayer;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceLayer.Interface
{
    public interface IUserRL
    {
        RegistrationModel RegisterUserRL(RegistrationModel registrationModel);
        LoginModel LoginUserRL(LoginModel loginModel);
        Task<UserEntity?> GetUserByEmailAsync(string email);
        Task<bool> SaveResetTokenAsync(string email, string token, DateTime expiry);
        Task<bool> ResetPasswordAsync(
    string email,
    string token,
    string newPassword);
    }
}
