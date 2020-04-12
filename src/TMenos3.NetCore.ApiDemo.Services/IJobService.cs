using System;
using System.Threading.Tasks;
using TMenos3.NetCore.ApiDemo.Models;

namespace TMenos3.NetCore.ApiDemo.Services
{
    public interface IJobService
    {
        Task<Guid> CreateAsync(Job job);

        Task<Job> GetAsync(Guid jobid);
    }
}
