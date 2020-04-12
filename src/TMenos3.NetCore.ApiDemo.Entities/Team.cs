﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace TMenos3.NetCore.ApiDemo.Entities
{
    public class Team
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public int LeagueId { get; set; }

        public string Name { get; set; }

        public string ShortName { get; set; }

        public string AreaName { get; set; }

        public string EMail { get; set; }

        public League League { get; set; }

        public ICollection<Player> Players { get; set; }
    }
}
