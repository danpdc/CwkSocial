namespace CwkSocial.Api.Contracts.Posts.Responses;

public class InteractionUser
{
    public Guid UserProfileId { get; set; }
    public string FullName { get; set; }
    public string City { get; set; }
}