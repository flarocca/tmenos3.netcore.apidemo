using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using TMenos3.NetCore.ApiDemo.Database.Entities;

namespace TMenos3.NetCore.ApiDemo.Database.Repositories
{
    internal class TeamRepository : ITeamRepository
    {
        private readonly DbSet<Team> _teams;

        public TeamRepository(DbSet<Team> teams) =>
            _teams = teams;

        public void AddRange(IEnumerable<Team> teams)
        {
            var toAdd = teams
                .Where(team => _teams
                    .Any(t => t.Id == team.Id) == false);
            _teams.AddRange(toAdd);
        }
    }
}
