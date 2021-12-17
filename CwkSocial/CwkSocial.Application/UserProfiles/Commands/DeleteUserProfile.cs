using Cwk.Domain.Aggregates.UserProfileAggregate;
using CwkSocial.Application.Models;
using MediatR;

namespace CwkSocial.Application.UserProfiles.Commands
{
    public class DeleteUserProfile : IRequest<OperationResult<UserProfile>>
    {
        public Guid UserProfileId { get; set; }
    }
}
