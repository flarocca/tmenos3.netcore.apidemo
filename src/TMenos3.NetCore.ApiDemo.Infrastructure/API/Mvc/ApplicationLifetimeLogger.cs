using EnsureThat;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace TMenos3.NetCore.ApiDemo.Infrastructure.API.Mvc
{
    internal class ApplicationLifetimeLogger
    {
        private readonly IHostApplicationLifetime _applicationLifetime;
        private readonly ILogger<ApplicationLifetimeLogger> _logger;

        public ApplicationLifetimeLogger(
            IHostApplicationLifetime applicationLifetime,
            ILogger<ApplicationLifetimeLogger> logger)
        {
            Ensure.Any.IsNotNull(applicationLifetime, nameof(applicationLifetime));
            Ensure.Any.IsNotNull(logger, nameof(logger));

            _applicationLifetime = applicationLifetime;
            _logger = logger;
        }

        public void Setup()
        {
            var processId = Process.GetCurrentProcess().Id;

            _applicationLifetime.ApplicationStarted.Register(
                () => _logger.LogInformation(Constants.LogMessageWebHostStarted, processId));
            _applicationLifetime.ApplicationStopping.Register(
                () => _logger.LogInformation(Constants.LogMessageWebHostStopping, processId));
            _applicationLifetime.ApplicationStopped.Register(
                () => _logger.LogInformation(Constants.LogMessageWebHostStopped, processId));
        }
    }
}
