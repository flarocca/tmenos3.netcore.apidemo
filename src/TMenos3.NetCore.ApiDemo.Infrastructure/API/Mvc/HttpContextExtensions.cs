using EnsureThat;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Primitives;
using System.Linq;

namespace TMenos3.NetCore.ApiDemo.Infrastructure.API.Mvc
{
    public static class HttpContextExtensions
    {
        public static string GetHeaderOrDefault(this HttpContext context, string headerName, string defaultValue = null)
        {
            Ensure.Any.IsNotNull(context, nameof(context));
            Ensure.String.IsNotNullOrWhiteSpace(headerName, nameof(headerName));

            return context.Request?.Headers?.TryGetValue(headerName, out StringValues headerValue) == true ?
                headerValue.ToString().WhenNullOrEmpty(defaultValue) : defaultValue;
        }

        public static string GetRemoteIpAddress(this HttpContext context)
        {
            Ensure.Any.IsNotNull(context, nameof(context));

            return context.Request?.Headers?.GetCommaSeparatedValues(
                ForwardedHeadersDefaults.XForwardedForHeaderName)?.FirstOrDefault();
        }

        private static string WhenNullOrEmpty(this string str, string defaultValue)
            => string.IsNullOrEmpty(str) ? defaultValue : str;
    }
}
