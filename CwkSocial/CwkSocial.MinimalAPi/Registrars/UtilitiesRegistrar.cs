using CwkSocial.Application.UserProfiles.Queries;
using CwkSocial.MinimalAPi.Abstractions;
using MediatR;

namespace CwkSocial.MinimalAPi.Registrars
{
    public class UtilitiesRegistrar : IRegistrar
    {
        public void RegisterServices(WebApplicationBuilder builder)
        {
            builder.Services.AddAutoMapper(typeof(Program), typeof(GetAllUserProfiles));
            builder.Services.AddMediatR(typeof(GetAllUserProfiles));
        }
    }
}
