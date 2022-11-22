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
