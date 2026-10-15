using System;
using System.Collections.Generic;
using System.Text;

namespace ModelLayer.User.ResponseModel
{
    public class ResetPasswordResponseModel
    {
        public string Message { get; set; } = "Password reset successfully";
        public DateTime ResetAt { get; set; } = DateTime.UtcNow;
    }
}
