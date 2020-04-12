using System.ComponentModel.DataAnnotations;

namespace TMenos3.NetCore.ApiDemo.Database.Entities
{
    public class LeagueTeam
    {
        public int LeagueId { get; set; }

        public League League { get; set; }

        public int TeamId { get; set; }

        public Team Team { get; set; }
    }
}
