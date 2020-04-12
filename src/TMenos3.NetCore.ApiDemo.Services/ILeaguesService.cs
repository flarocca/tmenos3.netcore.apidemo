using System.Threading.Tasks;
using TMenos3.NetCore.ApiDemo.Models;

namespace TMenos3.NetCore.ApiDemo.Services
{
    public interface ILeaguesService
    {
        Task<League> GetLeagueAsync(string leagueCode);

        Task ImportAsync(string leagueCode);
    }
}
