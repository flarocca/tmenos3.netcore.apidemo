using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using System;
using System.Net.Http;
using TMenos3.NetCore.ApiDemo.Services.Helpers;
using TMenos3.NetCore.ApiDemo.Services.Infrastructure;

namespace TMenos3.NetCore.ApiDemo.Services.Extensions
{
    public static class Extensions
    {
        public static IServiceCollection AddCustomServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ILeaguesService, LeaguesService>()
                .AddScoped<IPlayersService, PlayersService>()
                .AddScoped<IJobService, JobService>()
                .AddScoped<IBatchFactory, BatchFactory>();

            services.AddHttpClient<IFootballDataService, FootballDataService>(client =>
              {
                  var settings = configuration
                      .GetSection(FootballDataSettings.FootballDataSettingsSection)
                      .Get<FootballDataSettings>();

                  client.BaseAddress = new Uri(settings.BaseUrl);
                  client.DefaultRequestHeaders.Add(settings.TokenHeader, settings.ApiToken);
              })
                .SetHandlerLifetime(TimeSpan.FromMinutes(5))
                .AddPolicyHandler(GetRetryPolicy())
                .AddPolicyHandler(GetCircuitBreakerPolicy());

            return services;
        }

        private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy() =>
            HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
                .WaitAndRetryAsync(5, retryAttempt =>
                    TimeSpan.FromMinutes(1));

        private static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy() =>
            HttpPolicyExtensions
                .HandleTransientHttpError()
                .CircuitBreakerAsync(5, TimeSpan.FromSeconds(30));
    }
}
