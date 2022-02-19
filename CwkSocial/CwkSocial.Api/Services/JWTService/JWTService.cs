using CwkSocial.Application.Services;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace CwkSocial.Api.Services.JWTService
{
    public class JWTService : IJWTService
    {
        private JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
        private readonly JwtSettings jwtSettings;
        private readonly byte[] key;

        public JWTService(IOptions<JwtSettings> jwtOptions)
        {
            jwtSettings = jwtOptions.Value;
            key = Encoding.ASCII.GetBytes(jwtSettings.SigningKey);
        }


        public SecurityToken CreateSecurityToken(ClaimsIdentity identity)
        {
            var tokenDescriptor = getTokenDescriptor(identity);

            return tokenHandler.CreateToken(tokenDescriptor);
        }

        public string WriteToken(SecurityToken token)
        {
            return tokenHandler.WriteToken(token);
        }

        private SecurityTokenDescriptor getTokenDescriptor(ClaimsIdentity identity)
        {
            return new SecurityTokenDescriptor()
            {
                Subject = identity,
                Expires = DateTime.Now.AddHours(2),
                Audience = jwtSettings.Audiences[0],
                Issuer = jwtSettings.Issuer,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };
        }

        public string GetJwtString(IdentityUser identityUser, UserProfile userProfile)
        {
            var claimsIdentity = new ClaimsIdentity(new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, identityUser.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, identityUser.Email),
                new Claim("IdentityId", identityUser.Id),
                new Claim("UserProfileId", userProfile.UserProfileId.ToString())
            });

            var token = CreateSecurityToken(claimsIdentity);
            return WriteToken(token);
        }

    }
}
