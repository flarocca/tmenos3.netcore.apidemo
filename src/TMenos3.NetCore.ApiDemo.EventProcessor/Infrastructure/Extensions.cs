using Microsoft.Extensions.DependencyInjection;
using TMenos3.NetCore.ApiDemo.EventProcessor.Handlers;

namespace TMenos3.NetCore.ApiDemo.EventProcessor.Infrastructure
{
    internal static class Extensions
    {
        public static IServiceCollection AddHandlers(this IServiceCollection services) =>
            services.AddTransient<ImportLeagueCommandHandler>();
    }
}
