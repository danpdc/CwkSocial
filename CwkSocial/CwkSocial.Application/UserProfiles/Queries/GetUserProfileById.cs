using Cwk.Domain.Aggregates.UserProfileAggregate;
using CwkSocial.Application.Models;
using CwkSocial.Application.UserProfiles.Models;
using MediatR;

namespace CwkSocial.Application.UserProfiles.Queries
{
    public class GetUserProfileById : IRequest<OperationResult<UserProfileDto>>
    {
        public Guid UserProfileId { get; set; }
    }
}
