using DataService;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopBridgeApi.Config.Database
{
    public static class DbContextInitializer
    {
        public static void AddDbContexts(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddEntityFrameworkSqlServer()
                .AddDbContext<Shop_Bridge_dbContext>((serviceProvider, options) => {
                    options
                        .UseSqlServer(configuration.GetConnectionString("ShopBridgeDb"))
                        .UseInternalServiceProvider(serviceProvider);
                });
        }
    }
}
