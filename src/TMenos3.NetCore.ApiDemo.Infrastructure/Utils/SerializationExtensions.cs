using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace TMenos3.NetCore.ApiDemo.Infrastructure.Utils
{
    internal static class SerializationExtensions
    {
        public static string SerializeToJson(this object obj, IContractResolver contractResolver = null)
            => JsonConvert.SerializeObject(obj, GetJsonSerializerSettings(contractResolver));

        private static JsonSerializerSettings GetJsonSerializerSettings(IContractResolver contractResolver) =>
            new JsonSerializerSettings
            {
                ContractResolver = contractResolver ?? new CamelCasePropertyNamesContractResolver()
            };
    }
}
