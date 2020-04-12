using TMenos3.NetCore.ApiDemo.Infrastructure.EventBus.Messages;
using System.Threading.Tasks;

namespace TMenos3.NetCore.ApiDemo.Infrastructure.EventBus.Abstractions
{
    public interface IEventHandler<in TEvent>
        where TEvent : class, IEvent
    {
        Task HandleAsync(TEvent @event);
    }
}
