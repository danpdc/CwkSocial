namespace CwkSocial.Api.Contracts.Posts.Responses;

public class PostResponse
{
    public Guid PostId { get; set; }
    public Guid UserProfileId { get; set; }
    public string TextContent { get;  set; }
    public DateTime CreatedDate { get; set; }
    public DateTime LastModified { get; set; }
}