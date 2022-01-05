using System.ComponentModel.DataAnnotations;

namespace CwkSocial.Api.Contracts.Posts.Requests;

public class PostUpdate
{
    [Required]
    public string Text { get; set; }
    public string UserProfileId { get; set; }
}