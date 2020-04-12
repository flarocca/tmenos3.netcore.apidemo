using RabbitMQ.Client;
using System;

namespace TMenos3.NetCore.ApiDemo.Infrastructure.EventBus.RabbitMQ
{
    public interface IRabbitMQPersistentConnection : IDisposable
    {
        bool IsConnected { get; }

        bool TryConnect();

        IModel CreateModel();
    }
}
