namespace CwkSocial.MinimalAPi.Contracts.Post.Responses;

public class InteractionUser
{
    public Guid UserProfileId { get; set; }
    public string? FullName { get; set; }
    public string? City { get; set; }
}