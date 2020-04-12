using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace TMenos3.NetCore.ApiDemo.Infrastructure.Cache
{
    public static class Extensions
    {
        public static IServiceCollection AddDistributedCache(
            this IServiceCollection services,
            IConfiguration configuration,
            IHostEnvironment environment)
        {
            if (environment.IsDevelopment())
            {
                services.AddDistributedMemoryCache();
            }
            else
            {
                services.AddDistributedRedisCache(options =>
                {
                    options.Configuration = configuration.GetConnectionString("Redis");
                });
            }

            return services;
        }
    }
}
