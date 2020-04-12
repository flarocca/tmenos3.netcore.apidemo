using EnsureThat;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Extensions.Logging;
using Serilog.Sinks.Elasticsearch;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using TMenos3.NetCore.ApiDemo.Infrastructure.API.Mvc;
using TMenos3.NetCore.ApiDemo.Infrastructure.Utils;

namespace TMenos3.NetCore.ApiDemo.Infrastructure.Logging
{
    public static class Extensions
    {
        internal static IServiceCollection AddCustomLogging(
            this IServiceCollection services, IConfiguration configuration, IHostEnvironment hostEnvironment) =>
            services.AddLogging(loggingBuilder =>
                {
                    var logger = CreateLogger(
                        configuration,
                        hostEnvironment);

                    loggingBuilder.Services.AddSingleton<ILoggerFactory>(
                            provider => new SerilogLoggerFactory(logger, dispose: false));
                });

        public static IApplicationBuilder UseCorrelationContext(this IApplicationBuilder builder) =>
            builder.UseMiddleware<CorrelationContextMiddleware>();

        internal static double GetElapsed(this Stopwatch stopwatch)
        {
            Ensure.Any.IsNotNull(stopwatch, nameof(stopwatch));

            stopwatch.Stop();

            return stopwatch.Elapsed.TotalMilliseconds;
        }

        internal static string LogSerialize(this object obj)
        {
            Ensure.Any.IsNotNull(obj, nameof(obj));

            return obj.SerializeToJson(SkipLogContractResolver.Instance);
        }

        internal static IEnumerable<KeyValuePair<string, object>> GetLogScope(
            this CorrelationContext correlationContext)
        {
            Ensure.Any.IsNotNull(correlationContext, nameof(correlationContext));

            return new List<KeyValuePair<string, object>>
            {
                GetCorrelationContextLogProperty(
                nameof(CorrelationContext.Id),
                correlationContext.Id)
            };
        }

        public static KeyValuePair<string, object> AsLogProperty(this string propertyName, object propertyValue)
            => new KeyValuePair<string, object>(propertyName, propertyValue);

        private static KeyValuePair<string, object> GetCorrelationContextLogProperty(string propertyName, object propertyValue)
            => GetContextLogProperty(nameof(CorrelationContext), propertyName, propertyValue);

        private static KeyValuePair<string, object> GetContextLogProperty(string contextName, string propertyName, object propertyValue)
            => $"{contextName}{propertyName}".AsLogProperty(propertyValue);

        private static Serilog.ILogger CreateLogger(
            IConfiguration configuration,
            IHostEnvironment hostEnvironment)
        {
            var loggerConfiguration = new LoggerConfiguration();
            loggerConfiguration
                .ReadFrom.Configuration(configuration)
                .Enrich.FromLogContext();

            if (!hostEnvironment.IsDevelopment())
            {
                var loggingOptions = configuration.GetSection(
                    LoggingOptions.LoggingOptionsSectionName).Get<LoggingOptions>();

                loggerConfiguration.WriteTo.Elasticsearch(
                    new ElasticsearchSinkOptions(new Uri(loggingOptions.Node))
                    {
                        AutoRegisterTemplate = true,
                        IndexFormat = $"{Assembly.GetExecutingAssembly().GetName().Name.ToLower().Replace(".", "-")}-{hostEnvironment.EnvironmentName?.ToLower().Replace(".", "-")}-{DateTime.UtcNow:yyyy-MM}"
                    });
            }

            loggerConfiguration
                .WriteTo.ColoredConsole(outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} | {Level} | CorrelationContextId: {CorrelationContextId} | RequestPath: {RequestPath} | Message:{Message} | Exception:{Exception} | Properties: {Properties}{NewLine}");

            return loggerConfiguration.CreateLogger();
        }
    }
}
