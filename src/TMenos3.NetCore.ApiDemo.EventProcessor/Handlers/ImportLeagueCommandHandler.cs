using System.Net;
using System.Threading.Tasks;
using TMenos3.NetCore.ApiDemo.Contracts.Commands;
using TMenos3.NetCore.ApiDemo.Database.UnitOfWork;
using TMenos3.NetCore.ApiDemo.Infrastructure.EventBus.Abstractions;
using TMenos3.NetCore.ApiDemo.Services;
using TMenos3.NetCore.ApiDemo.Services.Exceptions;

namespace TMenos3.NetCore.ApiDemo.EventProcessor.Handlers
{
    public class ImportLeagueCommandHandler : IEventHandler<ImportLeagueCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILeaguesService _leaguesService;

        public ImportLeagueCommandHandler(IUnitOfWork unitOfWork, ILeaguesService leaguesService)
        {
            _unitOfWork = unitOfWork;
            _leaguesService = leaguesService;
        }

        public async Task HandleAsync(ImportLeagueCommand command)
        {
            try
            {
                await _leaguesService.ImportAsync(command.LeagueCode);
                await _unitOfWork.JobRepository.UpdateStatusCodeAsync(command.Id, HttpStatusCode.Created);
            }
            catch (LeagueAlreadyImportedException)
            {
                await _unitOfWork.JobRepository.UpdateStatusCodeAsync(command.Id, HttpStatusCode.Conflict);
            }
            catch (ServiceTimeoutException)
            {
                await _unitOfWork.JobRepository.UpdateStatusCodeAsync(command.Id, HttpStatusCode.GatewayTimeout);
            }

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
