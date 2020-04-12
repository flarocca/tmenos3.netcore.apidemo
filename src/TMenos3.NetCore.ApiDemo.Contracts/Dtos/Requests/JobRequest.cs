using Microsoft.AspNetCore.Mvc;

namespace TMenos3.NetCore.ApiDemo.Contracts.Dtos.Requests
{
    public class JobRequest
    {
        public string LeagueCode { get; set; }

        public string CallbackUri { get; set; }
    }
}
