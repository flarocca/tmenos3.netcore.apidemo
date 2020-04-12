using Microsoft.OpenApi.Models;
using System;

namespace TMenos3.NetCore.ApiDemo.Infrastructure.API.Swagger
{
    public class SwaggerSettings
    {
        public const string SwaggerSettingsSectionName = "Swagger";

        public static SwaggerSettings Default => new SwaggerSettings
        {
            Version = "v1",
            Title = "Default API",
            Description = "Default API",
            TermsOfService = "https://example.com/temrs-of-service",
            UseAuthorization = false,
            Contact = SwaggerContactSettings.Default,
            License = SwaggerLicenseSettings.Default
        };

        public string Version { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string TermsOfService { get; set; }

        public bool UseAuthorization { get; set; }

        public SwaggerContactSettings Contact { get; set; }

        public SwaggerLicenseSettings License { get; set; }

        public OpenApiInfo ToOpenApi() =>
            new OpenApiInfo
            {
                Version = Version,
                Title = Title,
                Description = Description,
                TermsOfService = new Uri(TermsOfService),
                Contact = Contact.ToOpenApi(),
                License = License.ToOpenApi()
            };
    }
}
