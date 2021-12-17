using Cwk.Domain.Aggregates.UserProfileAggregate;
using CwkSocial.Application.Models;
using MediatR;


namespace CwkSocial.Application.UserProfiles.Queries
{
    public class GetAllUserProfiles : IRequest<OperationResult<IEnumerable<UserProfile>>>
    {
    }
}
