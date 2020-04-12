using Microsoft.OpenApi.Models;
using System;

namespace TMenos3.NetCore.ApiDemo.Infrastructure.API.Swagger
{
    public class SwaggerContactSettings
    {
        public static SwaggerContactSettings Default => new SwaggerContactSettings
        {
            Name = "Contact name",
            Url = "http://example.com",
            Email = "email@example.com"
        };

        public string Name { get; set; }

        public string Url { get; set; }

        public string Email { get; set; }

        internal OpenApiContact ToOpenApi() =>
            new OpenApiContact
            {
                Name = Name,
                Url = new Uri(Url),
                Email = Email
            };
    }
}
