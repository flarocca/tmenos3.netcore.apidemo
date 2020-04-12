using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Globalization;
using System.IO;
using StartupBase = TMenos3.NetCore.ApiDemo.Infrastructure.API.Mvc.StartupBase;

namespace TMenos3.NetCore.ApiDemo.Infrastructure.HostBuilder
{
    public static class HostBuilderUtils
    {
        public static IHostBuilder CreateWebHostBuilder<TStartup>(string[] args)
            where TStartup : StartupBase
                => Host.CreateDefaultBuilder(args)
                    .UseConfiguration()
                    .ConfigureWebHostDefaults(webBuilder =>
                    {
                        webBuilder.UseStartup<TStartup>();
                    });

        public static IHostBuilder CreateHostBuilder<THostedService>(string[] args, Action<HostBuilderContext, IServiceCollection> configureCustomServices = null)
            where THostedService : class, IHostedService
                => Host.CreateDefaultBuilder(args)
                    .UseConfiguration()
                    .ConfigureServices((hostContext, services) =>
                    {
                        services.AddHostedService<THostedService>();
                        configureCustomServices?.Invoke(hostContext, services);
                    });

        private static IHostBuilder UseConfiguration(this IHostBuilder builder) =>
            builder.ConfigureAppConfiguration((context, config) =>
            {
                config
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile(GetConfigurationFilePath(), optional: false, reloadOnChange: true)
                    .AddJsonFile(GetConfigurationFilePath($".{context.HostingEnvironment.EnvironmentName}"), optional: true, reloadOnChange: true)
                    .AddEnvironmentVariables();
            });

        private static string GetConfigurationFilePath(string environmentName = null)
            => string.Format(CultureInfo.InvariantCulture, "appsettings{0}.json", environmentName);
    }
}
