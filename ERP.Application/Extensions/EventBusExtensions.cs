using ERP.Application.branch.handler;
using ERP.Application.company.events;
using ERP.EventBus.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace ERP.Application.Extensions;

public static class EventBusExtensions
{
    public static IServiceProvider SubscribeToEvents(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var eventBus = scope.ServiceProvider.GetRequiredService<IEventBus>();

        // Subscribe to CompanyCreatedEvent
        Task.Run(async () =>
        {
            await eventBus.SubscribeAsync<CompanyCreatedEvent>(async (companyCreatedEvent) =>
            {
                using var handlerScope = serviceProvider.CreateScope();
                var handler = handlerScope.ServiceProvider.GetRequiredService<CommandCreateBranchHandler>();
                await handler.HandleAsync(companyCreatedEvent);
            });
        });

        return serviceProvider;
    }
}