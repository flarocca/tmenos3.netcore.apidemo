using Microsoft.EntityFrameworkCore;
using System;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using TMenos3.NetCore.ApiDemo.Database.Entities;

[assembly: InternalsVisibleTo("TMenos3.NetCore.ApiDemo.IntegartionTests")]
namespace TMenos3.NetCore.ApiDemo.Database.Repositories
{
    internal class JobRepository : IJobRepository
    {
        private readonly DbSet<Job> _jobs;

        public JobRepository(DbSet<Job> jobs) =>
            _jobs = jobs;

        public Task<bool> ExistsByCodeAsync(string code) =>
            _jobs.AnyAsync(job => job.LeagueCode == code);

        public Task<Job> GetAsync(Guid jobId) =>
            _jobs.FirstOrDefaultAsync(job => job.Id == jobId);

        public void Add(Job job) =>
            _jobs.Add(job);

        public async Task UpdateStatusCodeAsync(Guid id, HttpStatusCode statusCode)
        {
            var job = await _jobs.FirstOrDefaultAsync(job => job.Id == id);
            job.StatusCode = statusCode;

            _jobs.Update(job);
        }
    }
}
