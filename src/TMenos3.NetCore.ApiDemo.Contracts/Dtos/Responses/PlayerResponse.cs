using System;

namespace TMenos3.NetCore.ApiDemo.Contracts.Dtos.Responses
{
    public class PlayerResponse
    {
        public int Id { get; set; }

        public int TeamId { get; set; }

        public string Name { get; set; }

        public string Position { get; set; }

        public string CountryOfBirth { get; set; }

        public DateTime DateOfBirth { get; set; }
    }
}
