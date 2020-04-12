using System.Threading.Tasks;

namespace TMenos3.NetCore.ApiDemo.Services
{
    public interface IPlayersService
    {
        Task<int> GetTotalPlayersByLeagueAsync(string leagueCode);
    }
}
