using ModelLayer;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using ServiceLayer.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace ServiceLayer
{
    public class UserRl : IUserRL
    {
        private readonly FundooContext fundooContext;
        private readonly IPasswordHasher<UserEntity> hasher;

        public UserRl(
            FundooContext fundooContext,
            IPasswordHasher<UserEntity> hasher)
        {
            this.fundooContext = fundooContext;
            this.hasher = hasher;
        }
        public async Task<UserEntity?> GetUserByEmailAsync(string email)
        {
            return await fundooContext.Users
                .FirstOrDefaultAsync(x => x.email == email);
        }

        public RegistrationModel RegisterUserRL(RegistrationModel registrationModel)
        {
            UserEntity user = new UserEntity();

            user.FirstName = registrationModel.FirstName;
            user.LastName = registrationModel.LastName;
            user.phoneNumber = registrationModel.ContactNumber;

            user.email = registrationModel.email;
            user.userName = registrationModel.UserName;

            // Hash the password before storing it
            user.password = hasher.HashPassword(
                user,
                registrationModel.Password);

            fundooContext.Users.Add(user);
            fundooContext.SaveChanges();

            return registrationModel;
        }
        public async Task<bool> ResetPasswordAsync(
    string email,
    string token,
    string newPassword)
        {
            var user = await fundooContext.Users
                .FirstOrDefaultAsync(x => x.email == email);

            if (user == null)
            {
                return false;
            }

            if (user.ResetToken != token)
            {
                return false;
            }

            if (user.ResetTokenExpiry == null ||
                user.ResetTokenExpiry < DateTime.UtcNow)
            {
                return false;
            }

            user.password = hasher.HashPassword(user, newPassword);

            user.ResetToken = null;
            user.ResetTokenExpiry = null;

            await fundooContext.SaveChangesAsync();

            return true;
        }
        public LoginModel LoginUserRL(LoginModel loginModel)
        {
            var user = fundooContext.Users
                .FirstOrDefault(x => x.email == loginModel.email);

            if (user == null)
            {
                return null;
            }

            var passwordResult = hasher.VerifyHashedPassword(
                user,
                user.password,
                loginModel.password);

            if (passwordResult != Microsoft.AspNetCore.Identity.PasswordVerificationResult.Success)
            {
                return null;
            }

            return loginModel;
        }
        public async Task<bool> SaveResetTokenAsync(
    string email,
    string token,
    DateTime expiry)
        {
            var user = await fundooContext.Users
                .FirstOrDefaultAsync(x => x.email == email);

            if (user == null)
            {
                return false;
            }

            user.ResetToken = token;
            user.ResetTokenExpiry = expiry;

            await fundooContext.SaveChangesAsync();

            return true;
        }

    }
}