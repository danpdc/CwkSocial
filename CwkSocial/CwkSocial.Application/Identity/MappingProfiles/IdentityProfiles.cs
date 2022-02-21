using AutoMapper;
using Cwk.Domain.Aggregates.UserProfileAggregate;
using CwkSocial.Application.Identity.Dtos;

namespace CwkSocial.Application.Identity.MappingProfiles;

public class IdentityProfiles : Profile
{
    public IdentityProfiles()
    {
        CreateMap<UserProfile, IdentityUserProfileDto>()
            .ForMember(dest => dest.Phone, opt 
            => opt.MapFrom(src => src.BasicInfo.Phone))
            .ForMember(dest => dest.CurrentCity, opt 
            => opt.MapFrom(src => src.BasicInfo.CurrentCity))
            .ForMember(dest => dest.EmailAddress, opt 
            => opt.MapFrom(src => src.BasicInfo.EmailAddress))
            .ForMember(dest => dest.FirstName, opt 
            => opt.MapFrom(src => src.BasicInfo.FirstName))
            .ForMember(dest => dest.LastName, opt 
            => opt.MapFrom(src => src.BasicInfo.LastName))
            .ForMember(dest => dest.DateOfBirth, opt 
            => opt.MapFrom(src => src.BasicInfo.DateOfBirth));
    }
}