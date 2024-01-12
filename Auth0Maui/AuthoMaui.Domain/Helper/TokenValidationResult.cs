using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace AuthoMaui.Domain.Helper
{
    internal class TokenValidationResult
    {
        public ClaimsPrincipal User { get; set; }
        public SecurityTokenExpiredException SecurityTokenExpiredException { get; set; }
        public bool IsError => User == null && SecurityTokenExpiredException == null;
    }
}
