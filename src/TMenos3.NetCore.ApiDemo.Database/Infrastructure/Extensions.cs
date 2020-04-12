using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TMenos3.NetCore.ApiDemo.Database.DbContexts;
using TMenos3.NetCore.ApiDemo.Database.Infrastructure;
using TMenos3.NetCore.ApiDemo.Database.UnitOfWork;

namespace TMenos3.NetCore.ApiDemo.Database.Extensions
{
    public static class Extensions
    {
        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration) =>
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString(Constants.ConnectionStringName),
                    options => options.EnableRetryOnFailure()))
                        .AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();

        public static IApplicationBuilder UseDatabase(this IApplicationBuilder applicationBuilder, IConfiguration configuration)
        {
            var databaseSettings = configuration
                .GetSection(DatabaseSettings.SectionName)
                .Get<DatabaseSettings>();

            if (databaseSettings != null && databaseSettings.RunMigartions)
            {
                using var serviceScope = applicationBuilder.ApplicationServices.CreateScope();
                var appContext = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>();

                appContext?.Database.Migrate();
            }

            return applicationBuilder;
        }
    }
}
