using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace CwkSocial.Dal.Extensions;

internal static class ModelBuilderExtensions
{
    internal static void ApplyAllConfigurations(this ModelBuilder modelBuilder)
    {
        var typesToRegister = Assembly
            .GetExecutingAssembly()
            .GetTypes()
            .Where(t => t.GetInterfaces()
                       .Any(gi => gi.IsGenericType &&
                                  gi.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>))).ToList();

        foreach (var type in typesToRegister)
        {
            dynamic configurationInstance = Activator.CreateInstance(type);
            modelBuilder.ApplyConfiguration(configurationInstance);
        }
    }
}