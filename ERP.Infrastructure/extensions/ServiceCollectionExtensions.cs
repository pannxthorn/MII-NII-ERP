using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace ERP.Infrastructure.extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();

            // Auto-register all repository implementations
            var repositoryTypes = assembly.GetTypes()
                .Where(type => type.IsClass && !type.IsAbstract &&
                              type.Name.EndsWith("Repository"))
                .ToList();

            foreach (var implementationType in repositoryTypes)
            {
                var interfaceType = implementationType.GetInterfaces()
                    .FirstOrDefault(i => i.Name == $"I{implementationType.Name}");

                if (interfaceType != null)
                {
                    services.AddScoped(interfaceType, implementationType);
                }
            }

            return services;
        }
    }
}
