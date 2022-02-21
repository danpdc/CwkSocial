namespace CwkSocial.Api.Contracts.Posts.Responses;

public class PostCommentResponse
{
    public Guid CommentId { get; set; }
    public string Text { get;  set; }
    public string UserProfileId { get; set; }
}