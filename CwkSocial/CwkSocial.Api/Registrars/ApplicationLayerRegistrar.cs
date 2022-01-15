using CwkSocial.Application.Services;

namespace CwkSocial.Api.Registrars;

public class ApplicationLayerRegistrar : IWebApplicationBuilderRegistrar
{
    public void RegisterServices(WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IdentityService>();
    }
}