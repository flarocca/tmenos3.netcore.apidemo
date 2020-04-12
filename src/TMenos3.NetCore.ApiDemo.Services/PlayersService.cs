using System.Threading.Tasks;
using TMenos3.NetCore.ApiDemo.Database.UnitOfWork;

namespace TMenos3.NetCore.ApiDemo.Services
{
    public class PlayersService : IPlayersService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PlayersService(IUnitOfWork unitOfWork) =>
            _unitOfWork = unitOfWork;

        public Task<int> GetTotalPlayersByLeagueAsync(string leagueCode) =>
            _unitOfWork.LeagueRepository.GetTotalPlayersAsync(leagueCode);
    }
}
