namespace CwkSocial.MinimalAPi.Contracts.Post.Responses;

public class PostInteraction
{
    public Guid InteractionId { get; set; }
    public string? Type { get; set; }
    public InteractionUser? Author { get; set; }
}