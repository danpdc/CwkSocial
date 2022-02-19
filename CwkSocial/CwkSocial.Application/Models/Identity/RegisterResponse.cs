using Cwk.Domain.Aggregates.UserProfileAggregate;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CwkSocial.Application.Models.Identity
{
    public class RegisterResponse
    {
        public RegisterResponse(IdentityUser identityUser, UserProfile userProfile)
        {
            IdentityUser = identityUser;
            UserProfile = userProfile;
        }

        public IdentityUser IdentityUser { get; private set; }
        public UserProfile UserProfile { get; private set; }
    }
}
