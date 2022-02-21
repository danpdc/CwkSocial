namespace CwkSocial.Api.Contracts.Identity;

public class IdentityUserProfile
{
    public string UserName { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string EmailAddress { get; set; }
    public string Phone { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string CurrentCity { get; set; }
    public string Token { get; set; }
}