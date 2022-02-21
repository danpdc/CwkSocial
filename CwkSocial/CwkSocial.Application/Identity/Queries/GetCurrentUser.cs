using System.Security.Claims;
using CwkSocial.Application.Identity.Dtos;
using CwkSocial.Application.Models;
using MediatR;

namespace CwkSocial.Application.Identity.Queries;

public class GetCurrentUser : IRequest<OperationResult<IdentityUserProfileDto>>
{
    public Guid UserProfileId { get; set; }
    public ClaimsPrincipal ClaimsPrincipal { get; set; }
}