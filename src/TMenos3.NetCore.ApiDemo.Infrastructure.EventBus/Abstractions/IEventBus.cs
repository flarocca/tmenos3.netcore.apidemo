using TMenos3.NetCore.ApiDemo.Infrastructure.EventBus.Messages;
using System.Threading.Tasks;

namespace TMenos3.NetCore.ApiDemo.Infrastructure.EventBus.Abstractions
{
    public interface IEventBus
    {
        Task PublishAsync(IEvent @event);

        void Subscribe<T, TH>()
            where T : class, IEvent
            where TH : IEventHandler<T>;

        void Unsubscribe<T, TH>()
            where TH : IEventHandler<T>
            where T : class, IEvent;
    }
}
