using Microsoft.Extensions.Logging;

namespace TMenos3.NetCore.ApiDemo.Infrastructure.API.Mvc
{
    internal class Constants
    {
        public const string NotSpecified = "NOT_SPECIFIED";
        public const string ActionParametersLogProperty = "ActionParameters";

        public const string CorsPolicy = "CorsPolicy";
        public const string CorrelationIdHeader = "X-Correlation-Id";

        public const string LogMessageWebHostStarted = "Web host started in process #{ProcessId}.";
        public const string LogMessageWebHostStopping = "Web host stopping in process #{ProcessId}.";
        public const string LogMessageWebHostStopped = "Web host stopped in process #{ProcessId}.";

        public const string LogActionExecutionStarted = "Action execution started.";
        public const string LogActionExecutionCanceled = "Action execution canceled in {ActionElapsed}.";
        public const string LogActionExecutionFailed = "Action execution failed in {ActionElapsed}.";
        public const string LogActionExecutionFinished = "Action execution finished in {ActionElapsed}.";

        public const string ErrorActionExceptionHandled = "Action exception handled.";

        public const string ErrorSimpleTypeModelBinderProviderNotFound = "Cannot find '{0}' in ASP.NET model binder providers.";

        public const string ContentTypeLogProperty = "ContentType";
        public const string ActionResultLogProperty = "ActionResult";
        public const string FileLengthLogProperty = "FileLength";

        public const LogLevel DetailedLogLevel = LogLevel.Information;
    }
}
