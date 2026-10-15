
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ModelLayer.User.RequestModel;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface.User;

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

        public RegistrationModel RegisterUserRL(
            RegistrationModel registrationModel)
        {
            UserEntity user = new UserEntity
            {
                FirstName = registrationModel.FirstName,
                LastName = registrationModel.LastName,
                phoneNumber = registrationModel.ContactNumber,
                email = registrationModel.email,
                userName = registrationModel.UserName
            };

            // Hash the password before storing it.
            user.password = hasher.HashPassword(
                user,
                registrationModel.Password);

            fundooContext.Users.Add(user);
            fundooContext.SaveChanges();

            return registrationModel;
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

            if (passwordResult != PasswordVerificationResult.Success)
            {
                return null;
            }

            return new LoginModel
            {
                email = user.email,
                password = string.Empty
            };
        }

        public async Task<(LoginModel user, int userId, string userName)>
            GetLoginUserWithIdAsync(string email, string password)
        {
            var user = await fundooContext.Users
                .FirstOrDefaultAsync(x => x.email == email);

            if (user == null)
            {
                return (null, 0, "");
            }

            var passwordResult = hasher.VerifyHashedPassword(
                user,
                user.password,
                password);

            if (passwordResult != PasswordVerificationResult.Success)
            {
                return (null, 0, "");
            }

            var loginModel = new LoginModel
            {
                email = user.email,
                password = string.Empty
            };

            return (loginModel, user.Id, user.userName);
        }

        // Called only after the Business Layer validates the reset JWT.
        public async Task<bool> UpdatePasswordAsync(
            string email,
            string newPassword)
        {
            var user = await fundooContext.Users
                .FirstOrDefaultAsync(x => x.email == email);

            if (user == null)
            {
                return false;
            }

            user.password = hasher.HashPassword(
                user,
                newPassword);

            await fundooContext.SaveChangesAsync();

            return true;
        }
    }
}
