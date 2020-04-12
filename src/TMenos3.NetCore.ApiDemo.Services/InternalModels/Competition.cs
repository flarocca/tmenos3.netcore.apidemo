using System.Collections.Generic;

namespace TMenos3.NetCore.ApiDemo.Services.InternalModels
{
    internal class Competition
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        public Area Area { get; set; }

        public IEnumerable<int> Teams { get; set; }
    }
}
