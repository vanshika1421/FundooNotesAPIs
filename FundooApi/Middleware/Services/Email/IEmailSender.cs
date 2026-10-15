using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Middleware.Services.Email
{
    public interface IEmailSender
    {
      
        Task SendEmailAsync(string to, string subject, string body);
    }
}
