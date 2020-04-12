using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;
using TMenos3.NetCore.ApiDemo.Database.Extensions;
using TMenos3.NetCore.ApiDemo.Infrastructure.Cache;
using TMenos3.NetCore.ApiDemo.Infrastructure.EventBus.RabbitMQ;
using TMenos3.NetCore.ApiDemo.Services;
using TMenos3.NetCore.ApiDemo.Services.Extensions;
using StartupBase = TMenos3.NetCore.ApiDemo.Infrastructure.API.Mvc.StartupBase;

namespace TMenos3.NetCore.ApiDemo.API
{
    public class Startup : StartupBase
    {
        public Startup(IConfiguration configuration, IHostEnvironment hostEnvironment)
            : base(configuration, hostEnvironment)
        {
        }

        protected override void ConfigureServices(IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
        {
            base.ConfigureServices(services, configuration, environment);

            services.AddCustomServices(configuration)
                    .AddRabbitMQEventBus(options =>
                    {
                        var settings = configuration.GetSection(RabbitMQOptions.RabbitMQOptionsSection)
                            .Get<RabbitMQOptions>();
                        options.HostName = settings.HostName;
                        options.SubscriptionName = settings.SubscriptionName;
                    })
                    .AddDistributedCache(configuration, environment);
        }

        protected override void Configure(IApplicationBuilder app, IConfiguration configuration, IHostEnvironment environment)
        {
            base.Configure(app, configuration, environment);

            app.UseDatabase(configuration);
        }

        protected override Assembly[] GetAssemblies() =>
            new Assembly[]
            {
                typeof(Startup).Assembly,
                typeof(ILeaguesService).Assembly
            };
    }
}
