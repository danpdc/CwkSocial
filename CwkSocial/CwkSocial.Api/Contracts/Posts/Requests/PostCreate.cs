using System.ComponentModel.DataAnnotations;

namespace CwkSocial.Api.Contracts.Posts.Requests;

public class PostCreate
{
    [Required]
    public string UserProfileId { get; set; }
    
    [Required]
    public string TextContent { get; set; }
}