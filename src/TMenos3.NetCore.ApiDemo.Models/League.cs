using System.Collections.Generic;

namespace TMenos3.NetCore.ApiDemo.Models
{
    public class League
    {
        public int Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public string AreaName { get; set; }

        public IReadOnlyList<Team> Teams { get; set; }
    }
}
