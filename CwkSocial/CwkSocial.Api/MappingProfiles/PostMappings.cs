using AutoMapper;
using Cwk.Domain.Aggregates.PostAggregate;
using CwkSocial.Api.Contracts.Posts.Responses;

namespace CwkSocial.Api.MappingProfiles;

public class PostMappings : Profile
{
    public PostMappings()
    {
        CreateMap<Post, PostResponse>();
        CreateMap<PostComment, PostCommentResponse>();
    }
}