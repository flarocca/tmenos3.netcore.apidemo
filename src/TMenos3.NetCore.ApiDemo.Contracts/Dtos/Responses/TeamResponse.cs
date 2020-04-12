using System.Collections.Generic;

namespace TMenos3.NetCore.ApiDemo.Contracts.Dtos.Responses
{
    public class TeamResponse
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string ShortName { get; set; }

        public string AreaName { get; set; }

        public string EMail { get; set; }

        public ICollection<PlayerResponse> Players { get; set; }
    }
}
