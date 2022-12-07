using CwkSocial.MinimalAPi.Abstractions;
using FluentValidation;

namespace CwkSocial.MinimalAPi.Registrars;

public class ValidatorsRegistrar : IRegistrar
{
    public void RegisterServices(WebApplicationBuilder builder)
    {
        builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);
    }
}