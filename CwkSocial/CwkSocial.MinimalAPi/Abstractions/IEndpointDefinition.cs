namespace CwkSocial.MinimalAPi.Abstractions;

public interface IEndpointDefinition
{
    void RegisterEndpoints(WebApplication app);
}