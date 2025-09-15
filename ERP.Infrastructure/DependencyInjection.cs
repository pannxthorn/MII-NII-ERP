using ERP.Application.unitofwork;
using ERP.Infrastructure.extensions;
using ERP.Infrastructure.unitofwork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // Database
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            // Unit of Work
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Auto-register all repositories
            services.AddRepositories();

            return services;
        }
    }
}
