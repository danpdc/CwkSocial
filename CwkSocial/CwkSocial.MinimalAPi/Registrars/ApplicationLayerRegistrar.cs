using CwkSocial.Application.Services;
using CwkSocial.MinimalAPi.Abstractions;

namespace CwkSocial.MinimalAPi.Registrars
{
    public class ApplicationLayerRegistrar : IRegistrar
    {
        public void RegisterServices(WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IdentityService>();
        }
    }
}
