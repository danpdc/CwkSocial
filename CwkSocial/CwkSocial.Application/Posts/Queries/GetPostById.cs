using Cwk.Domain.Aggregates.PostAggregate;
using CwkSocial.Application.Models;
using MediatR;

namespace CwkSocial.Application.Posts.Queries;

public class GetPostById : IRequest<OperationResult<Post>>
{
    public Guid PostId { get; set; }
}