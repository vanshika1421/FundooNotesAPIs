using System;
using System.Collections.Generic;
using System.Text;

namespace ModelLayer
{
    public class RegistrationResponseModel
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Message { get; set; } = "Registration successful";
    }
}
