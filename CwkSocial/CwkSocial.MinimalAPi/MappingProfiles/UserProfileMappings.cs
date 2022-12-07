using AutoMapper;
using Cwk.Domain.Aggregates.UserProfileAggregate;
using CwkSocial.Application.UserProfiles.Commands;
using CwkSocial.MinimalAPi.Contracts.Post.Responses;
using CwkSocial.MinimalAPi.Contracts.UserProfile.Requests;
using CwkSocial.MinimalAPi.Contracts.UserProfile.Responses;

namespace CwkSocial.MinimalAPi.MappingProfiles;

public class UserProfileMappings : Profile
{
    public UserProfileMappings()
    {
        CreateMap<UserProfileCreateUpdate, UpdateUserProfileBasicInfo>();
        CreateMap<UserProfile, UserProfileResponse>();
        CreateMap<BasicInfo, BasicInformation>();
        CreateMap<UserProfile, InteractionUser>()
            .ForMember(dest => dest.FullName, opt 
                => opt.MapFrom(src 
                    => src.BasicInfo.FirstName + " " + src.BasicInfo.LastName))
            .ForMember(dest => dest.City, opt 
                => opt.MapFrom(src => src.BasicInfo.CurrentCity));
    }
}