using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace CwkSocial.Api.Services.JWTService
{
    public interface IJWTService
    {
        SecurityToken CreateSecurityToken(ClaimsIdentity identity);
        string GetJwtString(IdentityUser identityUser, UserProfile userProfile);
        string WriteToken(SecurityToken token);
    }
}