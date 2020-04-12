using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace TMenos3.NetCore.ApiDemo.Database.Infrastructure
{
    internal static class DistributedCacheExtensions
    {
        public static async Task<T> GetOrCreateAsync<T>(this IDistributedCache cache, string key, Func<Task<T>> valueFactoryAsync)
        {
            var resultFromCache = await cache.GetStringAsync(key);
            if (string.IsNullOrWhiteSpace(resultFromCache))
            {
                var values = await valueFactoryAsync();
                await cache.SetStringAsync(key, JsonConvert.SerializeObject(values));
                return values;
            }

            return JsonConvert.DeserializeObject<T>(resultFromCache);
        }
    }
}
