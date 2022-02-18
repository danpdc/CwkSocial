using Cwk.Domain.Aggregates.PostAggregate;
using CwkSocial.Application.Models;
using MediatR;

namespace CwkSocial.Application.Posts.Commands;

public class UpdatePostComment : IRequest<OperationResult<PostComment>>
{
    public Guid UserProfileId { get; set; }
    public Guid PostId { get; set; }
    public Guid CommentId { get; set; }
    public string UpdatedText { get; set; }
}