using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace TMenos3.NetCore.ApiDemo.Infrastructure.API.Swagger
{
    public static class SwaggerExtensions
    {
        public static IServiceCollection AddSwaggerApiVersioning(this IServiceCollection services, IConfiguration configuration)
        {
            var swaggerSettings = configuration
                .GetSection(SwaggerSettings.SwaggerSettingsSectionName)
                .Get<SwaggerSettings>() ?? SwaggerSettings.Default;

            return services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc(swaggerSettings.Version, swaggerSettings.ToOpenApi());
            });
        }

        public static IApplicationBuilder UseSwaggerApiVersioning(this IApplicationBuilder app, IConfiguration configuration) =>
            app.UseSwagger(options =>
                {
                    options.RouteTemplate = Constants.SwaggerRouteTemplate;
                })
               .UseSwaggerUI(options =>
                {
                    var swaggerSettings = configuration
                        .GetSection(SwaggerSettings.SwaggerSettingsSectionName)
                        .Get<SwaggerSettings>() ?? SwaggerSettings.Default;

                    options.SwaggerEndpoint($"/swagger/{swaggerSettings.Version}/swagger.json", swaggerSettings.Title);
                    options.RoutePrefix = Constants.SwaggerRoutePrefix;
                });
    }
}
