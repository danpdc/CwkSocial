using Microsoft.AspNetCore.Identity;

namespace CwkSocial.Api.Services.JWTService
{
    public interface IJWTService
    {
        string GetJwtString(IdentityUser identityUser, UserProfile userProfile);
    }
}
