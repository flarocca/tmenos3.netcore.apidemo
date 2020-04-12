using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using TMenos3.NetCore.ApiDemo.Database.Entities;

namespace TMenos3.NetCore.ApiDemo.Database.Repositories
{
    internal class LeagueRepository : ILeagueRepository
    {
        private readonly DbSet<League> _leagues;

        public LeagueRepository(DbSet<League> leagues) =>
            _leagues = leagues;

        public Task<bool> ExistsByCodeAsync(string code) =>
            _leagues.AnyAsync(league => league.Code == code);

        public Task<League> GetByCodeAsync(string code) =>
            _leagues
                .Include(league => league.LeagueTeams)
                .ThenInclude(leagueTeam => leagueTeam.Team)
                .ThenInclude(team => team.Players)
                .FirstOrDefaultAsync(league => league.Code == code);

        public void Add(League league) =>
            _leagues.Add(league);

        public Task<int> GetTotalPlayersAsync(string leagueCode) =>
            _leagues
                .Where(league => league.Code == leagueCode)
                .Include(league => league.LeagueTeams)
                .ThenInclude(leagueTeam => leagueTeam.Team)
                .ThenInclude(team => team.Players)
                .SelectMany(league =>
                    league.LeagueTeams
                        .SelectMany(leagueTeam => leagueTeam.Team.Players))
                .CountAsync();
    }
}
