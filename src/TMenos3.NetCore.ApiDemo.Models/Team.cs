using System.Collections.Generic;

namespace TMenos3.NetCore.ApiDemo.Models
{
    public class Team
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string ShortName { get; set; }

        public string AreaName { get; set; }

        public string EMail { get; set; }

        public IReadOnlyList<Player> Players { get; set; }
    }
}
