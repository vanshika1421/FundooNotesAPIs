using System;
using System.Collections.Generic;
using System.Text;

namespace ModelLayer.User.ResponseModel
{
    public class ForgotPasswordResponseModel
    {
        public string Message { get; set; } = "Password reset email sent successfully";
        public DateTime SentAt { get; set; } = DateTime.UtcNow;
    }
}
