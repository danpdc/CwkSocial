namespace CwkSocial.Application.Identity;

public class IdentityErrorMessages
{
    public const string NonExistentIdentityUser = "Unable to find a user with the specified username";
    public const string IncorrectPassword = "The provided password is incorrect";
    public const string IdentityUserAlreadyExists = "Provided email address already exists. Cannot register new user";
}