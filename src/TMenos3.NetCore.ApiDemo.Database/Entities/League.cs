using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace TMenos3.NetCore.ApiDemo.Database.Entities
{
    public class League
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public string AreaName { get; set; }

        public IList<LeagueTeam> LeagueTeams { get; set; }
    }
}
