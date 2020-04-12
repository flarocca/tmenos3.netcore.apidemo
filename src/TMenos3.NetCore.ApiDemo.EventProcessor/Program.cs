using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.Reflection;
using TMenos3.NetCore.ApiDemo.Database.Extensions;
using TMenos3.NetCore.ApiDemo.EventProcessor.Infrastructure;
using TMenos3.NetCore.ApiDemo.Infrastructure.Cache;
using TMenos3.NetCore.ApiDemo.Infrastructure.EventBus.RabbitMQ;
using TMenos3.NetCore.ApiDemo.Infrastructure.HostBuilder;
using TMenos3.NetCore.ApiDemo.Services;
using TMenos3.NetCore.ApiDemo.Services.Extensions;

namespace TMenos3.NetCore.ApiDemo.EventProcessor
{
    public class Program
    {
        public static void Main(string[] args) =>
            CreateHostBuilder(args).Build().Run();

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            HostBuilderUtils.CreateHostBuilder<Worker>(args,
                (hostContext, services) =>
                {
                    services
                        .AddAutoMapper(GetAssemblies())
                        .AddDatabase(hostContext.Configuration)
                        .AddRabbitMQEventBus(options =>
                        {
                            var settings = hostContext.Configuration.GetSection(RabbitMQOptions.RabbitMQOptionsSection)
                                .Get<RabbitMQOptions>();
                            options.HostName = settings.HostName;
                            options.SubscriptionName = settings.SubscriptionName;
                        })
                        .AddHandlers()
                        .AddCustomServices(hostContext.Configuration)
                        .AddDistributedCache(hostContext.Configuration, hostContext.HostingEnvironment);
                });

        private static Assembly[] GetAssemblies() =>
            new Assembly[]
            {
                typeof(IJobService).Assembly
            };
    }
}
