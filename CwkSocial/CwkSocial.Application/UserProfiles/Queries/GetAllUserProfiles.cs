using Cwk.Domain.Aggregates.UserProfileAggregate;
using MediatR;


namespace CwkSocial.Application.UserProfiles.Queries
{
    public class GetAllUserProfiles : IRequest<IEnumerable<UserProfile>>
    {
    }
}
