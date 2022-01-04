using Cwk.Domain.Aggregates.PostAggregate;
using CwkSocial.Application.Models;
using MediatR;

namespace CwkSocial.Application.Posts.Queries;

public class GetAllPosts : IRequest<OperationResult<List<Post>>>
{
    
}