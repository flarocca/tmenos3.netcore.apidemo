using Newtonsoft.Json;
using System.IO;

namespace TMenos3.NetCore.ApiDemo.Services.Utils
{
    internal static class StreamExtensions
    {
        public static T DeserializeJson<T>(this Stream stream)
        {
            if (stream == null || stream.CanRead == false)
                return default;

            using var sr = new StreamReader(stream);
            using var jtr = new JsonTextReader(sr);

            var serializer = JsonSerializer.CreateDefault(new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });
            return serializer.Deserialize<T>(jtr);
        }
    }
}
