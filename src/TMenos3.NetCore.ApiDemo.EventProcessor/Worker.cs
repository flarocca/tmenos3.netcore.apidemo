using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;
using TMenos3.NetCore.ApiDemo.Contracts.Commands;
using TMenos3.NetCore.ApiDemo.EventProcessor.Handlers;
using TMenos3.NetCore.ApiDemo.Infrastructure.EventBus.Abstractions;

namespace TMenos3.NetCore.ApiDemo.EventProcessor
{
    public class Worker : BackgroundService
    {
        private readonly IEventBus _eventBus;

        public Worker(IEventBus eventBus) =>
            _eventBus = eventBus;

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _eventBus.Subscribe<ImportLeagueCommand, ImportLeagueCommandHandler>();
            return Task.CompletedTask;
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _eventBus.Unsubscribe<ImportLeagueCommand, ImportLeagueCommandHandler>();
            return Task.CompletedTask;
        }
    }
}
