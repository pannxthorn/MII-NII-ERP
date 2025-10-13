using ERP.Application.branch.handler;
using ERP.Application.company.events;
using ERP.EventBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ERP.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMediatR(configuration =>
            {
                configuration.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
            });

            services.AddAutoMapper(typeof(DependencyInjection).Assembly);

            // Add EventBus
            services.AddEventBus(configuration);

            // Register Event Handlers
            services.AddEventHandler<CompanyCreatedEvent, CommandCreateBranchHandler>();

            return services;
        }
    }
}
