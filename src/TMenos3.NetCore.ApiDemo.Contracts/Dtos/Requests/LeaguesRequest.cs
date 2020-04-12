using Microsoft.AspNetCore.Mvc;

namespace TMenos3.NetCore.ApiDemo.Contracts.Dtos.Requests
{
    public class LeaguesRequest
    {
        [FromRoute(Name = "leagueCode")]
        public string LeagueCode { get; set; }
    }
}
