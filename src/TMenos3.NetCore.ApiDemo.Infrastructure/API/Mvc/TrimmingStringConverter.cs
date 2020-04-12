using Newtonsoft.Json;
using System;

namespace TMenos3.NetCore.ApiDemo.Infrastructure.API.Mvc
{
    internal class TrimmingStringConverter : JsonConverter
    {
        public override bool CanRead => true;

        public override bool CanWrite => false;

        public override bool CanConvert(Type objectType) => objectType == typeof(string);

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var value = (reader.Value as string);
            return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            => throw new NotImplementedException();
    }
}
