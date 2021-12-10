using MediatR;

namespace CwkSocial.Application.UserProfiles.Commands
{
    public class DeleteUserProfile : IRequest
    {
        public Guid UserProfileId { get; set; }
    }
}
