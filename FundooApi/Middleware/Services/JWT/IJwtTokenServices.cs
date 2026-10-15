using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Middleware.Services.JWT
{
    public interface IJwtTokenServices
    {
        ClaimsPrincipal? ValidateToken(string token);
        string GenerateToken(string email , string userId);
    }
}
