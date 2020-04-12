using System.Threading.Tasks;
using TMenos3.NetCore.ApiDemo.Database.Entities;

namespace TMenos3.NetCore.ApiDemo.Database.Repositories
{
    public interface ILeagueRepository
    {
        Task<bool> ExistsByCodeAsync(string code);

        Task<League> GetByCodeAsync(string code);

        Task<int> GetTotalPlayersAsync(string leagueCode);

        void Add(League league);
    }
}
