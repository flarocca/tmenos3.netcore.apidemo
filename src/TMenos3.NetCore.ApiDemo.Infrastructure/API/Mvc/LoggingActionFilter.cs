using TMenos3.NetCore.ApiDemo.Infrastructure.Logging;
using EnsureThat;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Mime;
using System.Reflection;
using System.Threading.Tasks;

namespace TMenos3.NetCore.ApiDemo.Infrastructure.API.Mvc
{
    internal class LoggingActionFilter : IAsyncActionFilter
    {
        private readonly CorrelationContext _correlationContext;
        private readonly ILogger<LoggingActionFilter> _logger;

        public LoggingActionFilter(CorrelationContext correlationContext, ILogger<LoggingActionFilter> logger)
        {
            Ensure.Any.IsNotNull(correlationContext, nameof(correlationContext));
            Ensure.Any.IsNotNull(logger, nameof(logger));

            _correlationContext = correlationContext;
            _logger = logger;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            using (_logger.BeginScope(BuildActionContext(context)))
            {
                Lazy<List<KeyValuePair<string, object>>> actionParametersLog = BuildActionParametersLog(context);

                LogActionExecutionStarted(actionParametersLog);

                ActionExecutedContext result = await next();

                if (result.Canceled)
                {
                    LogActionExecutionCanceled(context, result, stopwatch);
                }
                else if (result.Exception != null)
                {
                    LogActionExecutionFailed(actionParametersLog, result, stopwatch);
                }
                else
                {
                    LogActionExecutionFinished(context, result, stopwatch);
                }
            }
        }

        private List<KeyValuePair<string, object>> BuildActionContext(ActionExecutingContext context)
        {
            string remoteIpAddress = context.HttpContext?.GetRemoteIpAddress() ?? Constants.NotSpecified;
            string userAgent = context.HttpContext?.GetHeaderOrDefault(HeaderNames.UserAgent, Constants.NotSpecified);

            var actionContext = new List<KeyValuePair<string, object>>
            {
                new KeyValuePair<string, object>(nameof(ConnectionInfo.RemoteIpAddress), remoteIpAddress),
                new KeyValuePair<string, object>(nameof(HeaderNames.UserAgent), userAgent)
            };

            actionContext.AddRange(_correlationContext.GetLogScope());

            return actionContext;
        }

        private Lazy<List<KeyValuePair<string, object>>> BuildActionParametersLog(ActionExecutingContext context)
        {
            return new Lazy<List<KeyValuePair<string, object>>>(() =>
            {
                var actionParameters = new Dictionary<string, object>();

                if (context.ActionDescriptor is ControllerActionDescriptor actionDescriptor)
                {
                    actionDescriptor.MethodInfo.GetParameters().ToList().ForEach(parameterInfo =>
                    {
                        if (parameterInfo.GetCustomAttribute<SkipLogAttribute>() != null)
                        {
                            return;
                        }

                        if (context.ActionArguments.TryGetValue(parameterInfo.Name, out object parameterValue) && parameterValue != null)
                        {
                            actionParameters.Add(parameterInfo.Name, parameterValue);
                        }
                    });
                }

                if (!actionParameters.Any())
                {
                    return null;
                }

                return new List<KeyValuePair<string, object>>
                {
                    Constants.ActionParametersLogProperty.AsLogProperty(actionParameters.LogSerialize())
                };
            });
        }

        private List<KeyValuePair<string, object>> BuildActionResultLog(ActionExecutingContext context, ActionExecutedContext result)
        {
            var actionResultLog = new List<KeyValuePair<string, object>>();

            if (result.Result is ObjectResult objectResult)
            {
                BuildObjectResultLog(actionResultLog, objectResult, context);
            }
            else if (result.Result is FileResult fileResult)
            {
                BuildFileResultLog(actionResultLog, fileResult);
            }

            return actionResultLog;
        }

        private void BuildObjectResultLog(
            List<KeyValuePair<string, object>> actionResultLog,
            ObjectResult objectResult,
            ActionExecutingContext context)
        {
            int statusCode = objectResult.StatusCode ?? StatusCodes.Status200OK;
            actionResultLog.Add(nameof(ObjectResult.StatusCode).AsLogProperty(statusCode));

            string contentType = objectResult.ContentTypes?.FirstOrDefault() ?? MediaTypeNames.Application.Json;
            actionResultLog.Add(Constants.ContentTypeLogProperty.AsLogProperty(contentType));

            if (objectResult.Value != null &&
                context.ActionDescriptor is ControllerActionDescriptor actionDescriptor &&
                actionDescriptor.MethodInfo.ReturnParameter.GetCustomAttribute<SkipLogAttribute>() == null)
            {
                actionResultLog.Add(Constants.ActionResultLogProperty.AsLogProperty(objectResult.Value.LogSerialize()));
            }
        }

        private void BuildFileResultLog(List<KeyValuePair<string, object>> actionResultLog, FileResult fileResult)
        {
            if (!string.IsNullOrEmpty(fileResult.ContentType))
            {
                actionResultLog.Add(Constants.ContentTypeLogProperty.AsLogProperty(fileResult.ContentType));
            }

            if (!string.IsNullOrEmpty(fileResult.FileDownloadName))
            {
                actionResultLog.Add(nameof(FileResult.FileDownloadName).AsLogProperty(fileResult.FileDownloadName));
            }

            if (fileResult.LastModified.HasValue)
            {
                actionResultLog.Add(nameof(FileResult.LastModified).AsLogProperty(fileResult.LastModified.Value));
            }

            StringSegment? entityTag = fileResult.EntityTag?.Tag;

            if (entityTag.HasValue && !string.IsNullOrEmpty(entityTag.Value.Value))
            {
                actionResultLog.Add(nameof(FileResult.EntityTag).AsLogProperty(entityTag.Value.Value));
            }

            long? fileLength = null;

            if (fileResult is FileStreamResult fileStreamResult)
            {
                if (fileStreamResult.FileStream?.CanSeek == true)
                {
                    fileLength = fileStreamResult.FileStream.Length;
                }
            }
            else if (fileResult is FileContentResult fileContentResult)
            {
                if (fileContentResult.FileContents != null)
                {
                    fileLength = fileContentResult.FileContents.Length;
                }
            }

            if (fileLength.HasValue)
            {
                actionResultLog.Add(Constants.FileLengthLogProperty.AsLogProperty(fileLength.Value));
            }
        }

        private void LogActionExecutionStarted(Lazy<List<KeyValuePair<string, object>>> actionParametersLog)
        {
            if (_logger.IsEnabled(Constants.DetailedLogLevel))
            {
                LogWithParameters(actionParametersLog.Value,
                    () => _logger.Log(Constants.DetailedLogLevel, Constants.LogActionExecutionStarted));
            }
        }

        private void LogActionExecutionCanceled(
            ActionExecutingContext context,
            ActionExecutedContext result,
            Stopwatch stopwatch)
        {
            if (_logger.IsEnabled(Constants.DetailedLogLevel))
            {
                LogWithParameters(
                    BuildActionResultLog(context, result),
                    () => _logger.Log(Constants.DetailedLogLevel, Constants.LogActionExecutionCanceled, stopwatch.GetElapsed()));
            }
        }

        private void LogActionExecutionFailed(
            Lazy<List<KeyValuePair<string, object>>> actionParametersLog,
            ActionExecutedContext result,
            Stopwatch stopwatch)
        {
            LogWithParameters(
                _logger.IsEnabled(Constants.DetailedLogLevel) ? null : actionParametersLog.Value,
                () => _logger.LogError(result.Exception, Constants.LogActionExecutionFailed, stopwatch.GetElapsed()));

            result.Exception = new HandledException(Constants.ErrorActionExceptionHandled, result.Exception);
        }

        private void LogActionExecutionFinished(
            ActionExecutingContext context,
            ActionExecutedContext result,
            Stopwatch stopwatch)
        {
            if (_logger.IsEnabled(Constants.DetailedLogLevel))
            {
                LogWithParameters(
                    BuildActionResultLog(context, result),
                    () => _logger.Log(Constants.DetailedLogLevel, Constants.LogActionExecutionFinished, stopwatch.GetElapsed()));
            }
        }

        private void LogWithParameters(List<KeyValuePair<string, object>> actionParametersLog, Action log)
        {
            if (actionParametersLog != null && actionParametersLog.Any())
            {
                using (_logger.BeginScope(actionParametersLog))
                {
                    log();
                }
            }
            else
            {
                log();
            }
        }
    }
}
