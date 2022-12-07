namespace CwkSocial.MinimalAPi.Contracts.Identity;

public class UserRegistration
{
    public string? Username { get; set; }
    public string? Password { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string? Phone { get; set; }
    public string? CurrentCity { get; set; }
}