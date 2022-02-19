namespace CwkSocial.Api.Contracts.Identity;

public class AuthenticationResult
{
    public AuthenticationResult(string token)
    {
        Token = token;
    }

    public string Token { get; }
}