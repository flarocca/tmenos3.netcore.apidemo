using Microsoft.OpenApi.Models;
using System;

namespace TMenos3.NetCore.ApiDemo.Infrastructure.API.Swagger
{
    public class SwaggerLicenseSettings
    {
        public static SwaggerLicenseSettings Default => new SwaggerLicenseSettings
        {
            Name = "MIT",
            Url = "http://example.com"
        };

        public string Name { get; set; }

        public string Url { get; set; }

        internal OpenApiLicense ToOpenApi() =>
            new OpenApiLicense
            {
                Name = Name,
                Url = new Uri(Url)
            };
    }
}
