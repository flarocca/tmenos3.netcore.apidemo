using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net;

namespace TMenos3.NetCore.ApiDemo.Database.Entities
{
    public class Job
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid Id { get; set; }

        public string LeagueCode { get; set; }

        public string CallbackUri { get; set; }

        public HttpStatusCode StatusCode { get; set; }
    }
}
