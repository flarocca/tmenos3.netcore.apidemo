using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace TMenos3.NetCore.ApiDemo.Entities
{
    public class Player
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public int TeamId { get; set; }

        public string Name { get; set; }

        public string Position { get; set; }

        public string CountryOfBirth { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string Nationality { get; set; }

        public Team Team { get; set; }
    }
}
