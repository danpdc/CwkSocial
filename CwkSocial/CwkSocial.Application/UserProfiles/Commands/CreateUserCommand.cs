using MediatR;
using Cwk.Domain.Aggregates.UserProfileAggregate;
using CwkSocial.Application.Models;

namespace CwkSocial.Application.UserProfiles.Commands
{
    public class CreateUserCommand : IRequest<OperationResult<UserProfile>>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string Phone { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string CurrentCity { get; set; }
    }
}
