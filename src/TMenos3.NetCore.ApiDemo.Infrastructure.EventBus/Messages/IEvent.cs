using Newtonsoft.Json;
using System;

namespace TMenos3.NetCore.ApiDemo.Infrastructure.EventBus.Messages
{
    public interface IEvent
    {
        [JsonProperty]
        Guid Id { get; }
    }
}
