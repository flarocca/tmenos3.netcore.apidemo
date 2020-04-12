using System.Collections.Generic;
using System.Threading.Tasks;
using TMenos3.NetCore.ApiDemo.Services.InternalModels;

namespace TMenos3.NetCore.ApiDemo.Services
{
    internal interface IFootballDataService
    {
        Task<Competition> GetFullCompetitionAsync(string competitionCode);

        Task<Team> GetTeamAsync(int teamId);
    }
}
