using System.ComponentModel.DataAnnotations;

namespace ModelLayer
{
    public class RegistrationModel
    {
        [Required]
        [MinLength(2)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [MinLength(2)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [MinLength(3)]
        public string UserName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string email { get; set; } = string.Empty;

        [Required]
        [RegularExpression(
            @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&]).{8,}$",
            ErrorMessage = "Password must contain 8 characters, uppercase, lowercase, number and special character."
        )]
        public string Password { get; set; } = string.Empty;

        [Required]
        [Phone]
        public string ContactNumber { get; set; } = string.Empty;
    }
}