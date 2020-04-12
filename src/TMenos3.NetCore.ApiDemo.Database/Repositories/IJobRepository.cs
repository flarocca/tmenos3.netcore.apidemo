using System;
using System.Net;
using System.Threading.Tasks;
using TMenos3.NetCore.ApiDemo.Database.Entities;

namespace TMenos3.NetCore.ApiDemo.Database.Repositories
{
    public interface IJobRepository
    {
        Task<bool> ExistsByCodeAsync(string code);

        Task<Job> GetAsync(Guid jobId);

        Task UpdateStatusCodeAsync(Guid id, HttpStatusCode statusCode);

        void Add(Job entityJob);
    }
}
