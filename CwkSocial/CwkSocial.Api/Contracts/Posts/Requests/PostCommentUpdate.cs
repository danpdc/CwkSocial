namespace CwkSocial.Api.Contracts.Posts.Requests;

public class PostCommentUpdate
{
    [Required]
    public string Text { get;  set; }
}