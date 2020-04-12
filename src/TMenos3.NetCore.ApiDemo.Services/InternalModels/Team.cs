using System.Collections.Generic;

namespace TMenos3.NetCore.ApiDemo.Services.InternalModels
{
    internal class Team
    {
        public int Id { get; set; }

        public Area Area { get; set; }

        public string Name { get; set; }

        public string ShortName { get; set; }

        public string Tla { get; set; }

        public string EMail { get; set; }

        public IEnumerable<Player> Squad { get; set; }
    }
}
