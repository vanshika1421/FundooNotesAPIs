using System;
using System.Collections.Generic;
using System.Text;

namespace ModelLayer.User.ResponseModel
{
    public class LoginResponseModel
    {
        public int UserId { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Token { get; set; }
        public string Message { get; set; } = "Login successful";
    }
}
