using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Reflection;

namespace TMenos3.NetCore.ApiDemo.Infrastructure.Logging
{
    internal class SkipLogContractResolver : CamelCasePropertyNamesContractResolver
    {
        public static readonly SkipLogContractResolver Instance = new SkipLogContractResolver();

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            JsonProperty property = base.CreateProperty(member, memberSerialization);

            if (member.GetCustomAttribute<SkipLogAttribute>() != null)
            {
                property.Ignored = true;
            }

            return property;
        }
    }
}
