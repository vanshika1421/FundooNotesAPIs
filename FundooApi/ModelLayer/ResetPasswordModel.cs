using System.ComponentModel.DataAnnotations;

namespace ModelLayer
{
    public class ResetPasswordModel
    {
       

        [Required]
        public string Token { get; set; } = string.Empty;

        [Required]
        public string NewPassword { get; set; } = string.Empty;
    }
}