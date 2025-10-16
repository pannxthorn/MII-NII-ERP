using ERP.Shared.authenticationservice;
using ERP.Shared.encryptservice;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Shared
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddSharedLayer(this IServiceCollection services)
        {
            services.AddScoped<IHashIdService, HashIdService>();
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IPasswordHashingService, PasswordHashingService>();
            return services;
        }
    }
}
