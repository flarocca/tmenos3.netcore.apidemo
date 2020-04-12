namespace TMenos3.NetCore.ApiDemo.Infrastructure.EventBus.RabbitMQ
{
    public class RabbitMQOptions
    {
        public const string RabbitMQOptionsSection = "EventBus";

        public string HostName { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string SubscriptionName { get; set; }

        public int RetryCount { get; set; }
    }
}
