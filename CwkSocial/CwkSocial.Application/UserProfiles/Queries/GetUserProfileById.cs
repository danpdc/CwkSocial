using Cwk.Domain.Aggregates.UserProfileAggregate;
using CwkSocial.Application.Models;
using MediatR;

namespace CwkSocial.Application.UserProfiles.Queries
{
    public class GetUserProfileById : IRequest<OperationResult<UserProfile>>
    {
        public Guid UserProfileId { get; set; }
    }
}
