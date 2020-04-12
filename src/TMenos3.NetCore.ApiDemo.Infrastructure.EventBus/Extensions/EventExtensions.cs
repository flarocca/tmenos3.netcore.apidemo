using Newtonsoft.Json;
using System.Text;
using TMenos3.NetCore.ApiDemo.Infrastructure.EventBus.Messages;

namespace TMenos3.NetCore.ApiDemo.Infrastructure.EventBus.Extensions
{
    public static class EventExtensions
    {
        public static byte[] ToBytes(this IEvent @event)
        {
            var message = JsonConvert.SerializeObject(@event);
            return Encoding.UTF8.GetBytes(message);
        }
    }
}
