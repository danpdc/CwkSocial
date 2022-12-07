using CwkSocial.MinimalAPi.Abstractions;

namespace CwkSocial.MinimalAPi.Extensions
{
    public static class RegistrarExtensions
    {
        public static void RegisterServices(this WebApplicationBuilder builder)
        {
            var registrars = GetRegistrars();

            foreach (var registrar in registrars)
            {
                registrar.RegisterServices(builder);
            }
        }
        
        public static void RegisterEndpointDefinitions(this WebApplication app)
        {
            var endpointDefinitions = typeof(Program).Assembly
                .GetTypes()
                .Where(t => t.IsAssignableTo(typeof(IEndpointDefinition)) && !t.IsAbstract && !t.IsInterface)
                .Select(Activator.CreateInstance)
                .Cast<IEndpointDefinition>();

            foreach (var endpointDef in endpointDefinitions)
            {
                endpointDef.RegisterEndpoints(app);
            }
        }

        private static IEnumerable<IRegistrar> GetRegistrars()
        {
            var scanningType = typeof(Program);
            return scanningType.Assembly.GetTypes()
                .Where(t => t.IsAssignableTo(typeof(IRegistrar)) && !t.IsAbstract && !t.IsInterface)
                .Select(Activator.CreateInstance)
                .Cast<IRegistrar>();
        }
    }
}
