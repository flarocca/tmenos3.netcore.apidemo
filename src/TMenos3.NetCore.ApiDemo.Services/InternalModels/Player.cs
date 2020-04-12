using System;

namespace TMenos3.NetCore.ApiDemo.Services.InternalModels
{
    internal class Player
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Position { get; set; }

        public string CountryOfBirth { get; set; }

        public string Nationality { get; set; }

        public DateTime DateOfBirth { get; set; }
    }
}
