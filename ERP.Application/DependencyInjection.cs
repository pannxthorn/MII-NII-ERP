using ERP.Application._base;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(configuration =>
            {
                configuration.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);

                // Add Pipeline Behavior สำหรับ inject CurrentUser
                configuration.AddOpenBehavior(typeof(CurrentUserBehavior<,>));
            });

            services.AddAutoMapper(typeof(DependencyInjection).Assembly);
            return services;
        }
    }
}
