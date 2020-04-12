using EnsureThat;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace TMenos3.NetCore.ApiDemo.Infrastructure.EventBus.RabbitMQ
{
    public static class Extensions
    {
        public static IServiceCollection AddRabbitMQEventBus(
            this IServiceCollection services,
            Action<RabbitMQOptions> optionsBuilder)
        {
            var options = new RabbitMQOptions();
            optionsBuilder(options);
            options = options.Validate();

            return services
                .AddEventBus(options)
                .AddRabbitMQPersistentConnection(options);
        }

        private static RabbitMQOptions Validate(this RabbitMQOptions options)
        {
            Ensure.String.IsNotNullOrWhiteSpace(options.HostName, nameof(options.HostName));
            Ensure.String.IsNotEmptyOrWhiteSpace(options.HostName, nameof(options.HostName));
            Ensure.String.IsNotEmptyOrWhiteSpace(options.SubscriptionName, nameof(options.SubscriptionName));
            Ensure.String.IsNotNullOrWhiteSpace(options.SubscriptionName, nameof(options.SubscriptionName));

            if(options.RetryCount != 0)
            {
                Ensure.Comparable.IsGt(options.RetryCount, 0, nameof(options.RetryCount));
            }

            return options;
        }
    }
}
