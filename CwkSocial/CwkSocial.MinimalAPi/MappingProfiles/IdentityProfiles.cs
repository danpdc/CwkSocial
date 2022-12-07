using AutoMapper;
using CwkSocial.Application.Identity.Commands;
using CwkSocial.Application.Identity.Dtos;
using CwkSocial.MinimalAPi.Contracts.Identity;

namespace CwkSocial.MinimalAPi.MappingProfiles;

public class IdentityMappings : Profile
{
    public IdentityMappings()
    {
        CreateMap<UserRegistration, RegisterIdentity>();
        CreateMap<Login, LoginCommand>();
        CreateMap<IdentityUserProfileDto, IdentityUserProfile>();
    }
}