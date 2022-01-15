using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CwkSocial.Application.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace CwkSocial.Application.Services;

public class IdentityService
{
    private readonly JwtSettings _jwtSettings;
    private readonly byte[] _key;

    public IdentityService(IOptions<JwtSettings> jwtOptions)
    {
        _jwtSettings = jwtOptions.Value;
        _key = Encoding.ASCII.GetBytes(_jwtSettings.SigningKey);
    }

    public JwtSecurityTokenHandler TokenHandler = new JwtSecurityTokenHandler();

    public SecurityToken CreateSecurityToken(ClaimsIdentity identity)
    {
        var tokenDescriptor = GetTokenDescriptor(identity);

        return TokenHandler.CreateToken(tokenDescriptor);
    }

    public string WriteToken(SecurityToken token)
    {
        return TokenHandler.WriteToken(token);
    }

    private SecurityTokenDescriptor GetTokenDescriptor(ClaimsIdentity identity)
    {
        return new SecurityTokenDescriptor()
        {
            Subject = identity,
            Expires = DateTime.Now.AddHours(2),
            Audience = _jwtSettings.Audiences[0],
            Issuer = _jwtSettings.Issuer,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(_key),
                SecurityAlgorithms.HmacSha256Signature)
        };
    }

}