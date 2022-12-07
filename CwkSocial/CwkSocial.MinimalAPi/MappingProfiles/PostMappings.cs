using AutoMapper;
using Cwk.Domain.Aggregates.PostAggregate;
using CwkSocial.MinimalAPi.Contracts.Post.Responses;
using PostInteraction = Cwk.Domain.Aggregates.PostAggregate.PostInteraction;

namespace CwkSocial.MinimalAPi.MappingProfiles;

public class PostMappings : Profile
{
    public PostMappings()
    {
        CreateMap<Post, PostResponse>();
        CreateMap<PostComment, PostCommentResponse>();
        CreateMap<PostInteraction, CwkSocial.MinimalAPi.Contracts.Post.Responses.PostInteraction>()
            .ForMember(dest 
                => dest.Type, opt 
                => opt.MapFrom(src 
                    => src.InteractionType.ToString()))
            .ForMember(dest => dest.Author, opt 
                => opt.MapFrom(src => src.UserProfile));
    }
}