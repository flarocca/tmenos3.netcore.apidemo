using System.Collections.Generic;
using TMenos3.NetCore.ApiDemo.Database.Entities;

namespace TMenos3.NetCore.ApiDemo.Database.Repositories
{
    public interface ITeamRepository
    {
        void AddRange(IEnumerable<Team> teams);
    }
}
