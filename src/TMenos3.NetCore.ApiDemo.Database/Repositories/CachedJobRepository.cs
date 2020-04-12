using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Net;
using System.Threading.Tasks;
using TMenos3.NetCore.ApiDemo.Database.Entities;
using TMenos3.NetCore.ApiDemo.Database.Infrastructure;

namespace TMenos3.NetCore.ApiDemo.Database.Repositories
{
    internal class CachedJobRepository : IJobRepository
    {
        private const string JOB_KEY_TEMPLATE = "JOB-{0}";
        private const string JOB_BY_CODE_KEY_TEMPLATE = "JOB-CODE-{0}";

        private readonly JobRepository _repository;
        private readonly IDistributedCache _cache;

        public CachedJobRepository(JobRepository repository, IDistributedCache cache)
        {
            _repository = repository;
            _cache = cache;
        }

        public Task<bool> ExistsByCodeAsync(string code) =>
            _cache.GetOrCreateAsync(
                string.Format(JOB_BY_CODE_KEY_TEMPLATE, code),
                () => _repository.ExistsByCodeAsync(code));

        public Task<Job> GetAsync(Guid jobId) =>
            _cache.GetOrCreateAsync(
                string.Format(JOB_KEY_TEMPLATE, jobId),
                () => _repository.GetAsync(jobId));

        public void Add(Job job) =>
            _repository.Add(job);

        public async Task UpdateStatusCodeAsync(Guid id, HttpStatusCode statusCode)
        {
            await _repository.UpdateStatusCodeAsync(id, statusCode);
            await _cache.RemoveAsync(string.Format(JOB_KEY_TEMPLATE, id));
        }
    }
}
