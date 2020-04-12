using Microsoft.OpenApi.Models;

namespace TMenos3.NetCore.ApiDemo.Infrastructure.API.Swagger
{
    internal class Constants
    {
        public const string SecurityDefinitionName = "oauth2";
        public const ParameterLocation SecurityDefinitionSchemeIn = ParameterLocation.Header;
        public const SecuritySchemeType SecurityDefinitionSchemeType = SecuritySchemeType.ApiKey;
        public const string SecurityDefinitionSchemeDescription = "Standard Authorization header using the Bearer scheme. Example: \"bearer {token}\"";
        public const string SecurityDefinitionSchemeName = "Authorization";
        public const string UnauthorizedResponseDescription = "Authorization token is invalid or not provided.";
        public const string ForbiddenResponseDescription = "Authorization token must have any scope from the list: {0}.";
        public const string ErrorEmptyListOfScopes = "The {0}.{1} action has empty effective list of allowed scopes.";
        public const string SwaggerSchemaTemplate = "#/definitions/{0}";
        public const string DdsAssemblyPrefix = "DDS.";
        public const string ApiNotFoundErrorTitle = "Specified API is not found.";

        public const string SwaggerRouteTemplate = "swagger/{documentName}/swagger.json";
        public const string SwaggerRoutePrefix = "swagger";
        public const string SwaggerEndpointUrl = "./{0}/swagger.json";
        public const string SwaggerLowercaseVersionMarker = "{version}";
        public const string SwaggerUppercaseVersionMarker = "{VERSION}";
    }
}
