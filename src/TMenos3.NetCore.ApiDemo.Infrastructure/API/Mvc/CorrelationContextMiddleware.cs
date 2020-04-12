using EnsureThat;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TMenos3.NetCore.ApiDemo.Infrastructure.API.Mvc
{
    internal class CorrelationContextMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CorrelationContextMiddleware> _logger;

        public CorrelationContextMiddleware(
            RequestDelegate next,
            ILogger<CorrelationContextMiddleware> logger)
        {
            Ensure.Any.IsNotNull(next, nameof(next));
            Ensure.Any.IsNotNull(logger, nameof(logger));

            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context, IServiceProvider serviceProvider)
        {
            var correlationContext = new CorrelationContext
            {
                Id = TryParseGuid(GetHeaderOrDefault(context, Constants.CorrelationIdHeader)) ?? Guid.NewGuid()
            };

            context.Response?.Headers?.Add(Constants.CorrelationIdHeader, correlationContext.Id.ToString());

            using (_logger.BeginScope(GetLogScope(correlationContext)))
            {
                await _next(context);
            }
        }

        private static List<KeyValuePair<string, object>> GetLogScope(
            CorrelationContext correlationContext)
        {
            Ensure.Any.IsNotNull(correlationContext, nameof(correlationContext));

            return new List<KeyValuePair<string, object>>
            {
                GetCorrelationContextLogProperty(
                nameof(CorrelationContext.Id),
                correlationContext.Id)
            };
        }

        private static KeyValuePair<string, object> GetCorrelationContextLogProperty(string propertyName, object propertyValue)
            => GetContextLogProperty(nameof(CorrelationContext), propertyName, propertyValue);

        private static KeyValuePair<string, object> GetContextLogProperty(string contextName, string propertyName, object propertyValue)
            => new KeyValuePair<string, object>($"{contextName}{propertyName}", propertyValue);

        public string GetHeaderOrDefault(HttpContext context, string headerName, string defaultValue = null)
        {
            Ensure.Any.IsNotNull(context, nameof(context));
            Ensure.String.IsNotNullOrWhiteSpace(headerName, nameof(headerName));

            return context.Request?.Headers?.TryGetValue(headerName, out StringValues headerValue) == true ?
                WhenNullOrEmpty(headerValue.ToString(), defaultValue) : defaultValue;
        }

        public string WhenNullOrEmpty(string str, string defaultValue)
            => string.IsNullOrEmpty(str) ? defaultValue : str;

        public Guid? TryParseGuid(string str)
            => Guid.TryParse(str, out Guid guid) ? guid : (Guid?)null;
    }
}
