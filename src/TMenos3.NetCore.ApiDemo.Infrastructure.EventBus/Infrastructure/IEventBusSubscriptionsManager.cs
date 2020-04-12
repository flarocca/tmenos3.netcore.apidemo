using TMenos3.NetCore.ApiDemo.Infrastructure.EventBus.Abstractions;
using TMenos3.NetCore.ApiDemo.Infrastructure.EventBus.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace TMenos3.NetCore.ApiDemo.Infrastructure.EventBus.Infrastructure
{
    public interface IEventBusSubscriptionsManager
    {
        bool IsEmpty { get; }
        event EventHandler<string> OnEventRemoved;

        void AddSubscription<T, TH>()
           where T : class, IEvent
           where TH : IEventHandler<T>;

        void RemoveSubscription<T, TH>()
             where TH : IEventHandler<T>
             where T : class, IEvent;

        bool HasSubscriptionsForEvent<T>() where T : IEvent;

        bool HasSubscriptionsForEvent(string eventName);

        Type GetEventTypeByName(string eventName);

        void Clear();

        IEnumerable<SubscriptionInfo> GetHandlersForEvent<T>() where T : IEvent;

        IEnumerable<SubscriptionInfo> GetHandlersForEvent(string eventName);

        string GetEventKey<T>();
    }
}
