using Cwk.Domain.Aggregates.PostAggregate;
using CwkSocial.Application.Models;
using MediatR;

namespace CwkSocial.Application.Posts.Commands;

public class UpdatePostCommentText : IRequest<OperationResult<PostComment>>
{
    public string NewText { get; set; }

    public Guid PostCommentId { get; set; }
    public Guid PostId { get; set; }
}

