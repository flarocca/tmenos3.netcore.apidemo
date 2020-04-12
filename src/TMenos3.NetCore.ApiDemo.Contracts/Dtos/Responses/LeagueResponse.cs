using System.Collections.Generic;

namespace TMenos3.NetCore.ApiDemo.Contracts.Dtos.Responses
{
    public class LeagueResponse
    {
        public int Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public string AreaName { get; set; }

        public IList<TeamResponse> Teams { get; set; }
    }
}
