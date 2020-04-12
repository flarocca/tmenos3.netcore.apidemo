using TMenos3.NetCore.ApiDemo.Infrastructure.EventBus.Abstractions;
using TMenos3.NetCore.ApiDemo.Infrastructure.EventBus.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System;

namespace TMenos3.NetCore.ApiDemo.Infrastructure.EventBus.RabbitMQ
{
    internal static class InternalExtensions
    {
        internal static IServiceCollection AddEventBus(this IServiceCollection services, RabbitMQOptions options) =>
            services.AddSingleton<IEventBus, EventBusRabbitMQ>(EventBusRabbitMQBuilder(options))
                    .AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();

        internal static IServiceCollection AddRabbitMQPersistentConnection(this IServiceCollection services, RabbitMQOptions options) =>
            services.AddSingleton(RabbitMQPersistentConnectionBuilder(options));

        private static Func<IServiceProvider, EventBusRabbitMQ> EventBusRabbitMQBuilder(RabbitMQOptions options) =>
            sp =>
            {
                var subscriptionClientName = options.SubscriptionName;
                var rabbitMQPersistentConnection = sp.GetRequiredService<IRabbitMQPersistentConnection>();
                var logger = sp.GetRequiredService<ILogger<EventBusRabbitMQ>>();
                var eventBusSubcriptionsManager = sp.GetRequiredService<IEventBusSubscriptionsManager>();

                var retryCount = options.RetryCount != 0 ? options.RetryCount : 5;

                return new EventBusRabbitMQ(rabbitMQPersistentConnection, logger, sp, eventBusSubcriptionsManager, subscriptionClientName, retryCount);
            };

        private static Func<IServiceProvider, IRabbitMQPersistentConnection> RabbitMQPersistentConnectionBuilder(
            RabbitMQOptions options) =>
                serviceProvider =>
                {
                    var logger = serviceProvider.GetRequiredService<ILogger<DefaultRabbitMQPersistentConnection>>();

                    var factory = new ConnectionFactory()
                    {
                        HostName = options.HostName,
                        DispatchConsumersAsync = true
                    };

                    if (!string.IsNullOrEmpty(options.Username))
                    {
                        factory.UserName = options.Username;
                    }

                    if (!string.IsNullOrEmpty(options.Password))
                    {
                        factory.Password = options.Password;
                    }

                    var retryCount = options.RetryCount != 0 ? options.RetryCount : 5;

                    return new DefaultRabbitMQPersistentConnection(factory, logger, retryCount);
                };
    }
}
