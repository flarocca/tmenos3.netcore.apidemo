using TMenos3.NetCore.ApiDemo.Infrastructure.API.Swagger;
using TMenos3.NetCore.ApiDemo.Infrastructure.Logging;
using EnsureThat;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Serialization;
using System;

namespace TMenos3.NetCore.ApiDemo.Infrastructure.API.Mvc
{
    public static class Extensions
    {
        public static void AddCustomApi(this IServiceCollection services, IConfiguration configuration, IHostEnvironment hostEnvironment)
        {
            Ensure.Any.IsNotNull(services, nameof(services));

            services
                .AddCustomLogging(configuration, hostEnvironment)
                .AddCustomControllers(configuration)
                .AddSwaggerApiVersioning(configuration)
                .AddCorsPolicy()
                .AddApplicationLifetimeLogging();
        }

        internal static IServiceCollection AddCorsPolicy(this IServiceCollection services) =>
            services.AddCors(options =>
            {
                options.AddPolicy(
                    Constants.CorsPolicy,
                    builder => builder
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials()
                        .SetIsOriginAllowed(_ => true)
                        .WithExposedHeaders(Constants.CorrelationIdHeader));
            });

        internal static IServiceCollection AddApplicationLifetimeLogging(this IServiceCollection services) =>
            services.AddSingleton<ApplicationLifetimeLogger>();

        internal static IServiceCollection AddCustomControllers(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddScoped<CorrelationContext>()
                .AddScoped<LoggingActionFilter>()
                .AddRouting(options => options.LowercaseUrls = true)
                .AddControllers(options =>
                {
                    options.AddStringTrimmingProvider();
                    options.Filters.AddService<LoggingActionFilter>(int.MinValue);
                })
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    options.SerializerSettings.Converters.Insert(0, new TrimmingStringConverter());
                }).SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            return services;
        }

        internal static void AddStringTrimmingProvider(this MvcOptions options)
        {
            var providers = options.ModelBinderProviders;

            for (int i = 0; i < providers.Count; i++)
            {
                if (providers[i].GetType() == typeof(SimpleTypeModelBinderProvider))
                {
                    providers.Insert(i, new TrimmingModelBinderProvider());
                    return;
                }
            }

            throw new InvalidOperationException(string.Format(Constants.ErrorSimpleTypeModelBinderProviderNotFound, nameof(SimpleTypeModelBinderProvider)));
        }
    }
}
