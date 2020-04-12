using System;
using System.Net;

namespace TMenos3.NetCore.ApiDemo.Models
{
    public class Job
    {
        public Guid Id { get; set; }

        public string LeagueCode { get; set; }

        public string CallbackUri { get; set; }

        public HttpStatusCode StatusCode { get; set; }
    }
}
