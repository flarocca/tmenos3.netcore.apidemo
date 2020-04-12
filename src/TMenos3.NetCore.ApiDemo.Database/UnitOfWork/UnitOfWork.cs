using Microsoft.Extensions.Caching.Distributed;
using System.Threading.Tasks;
using TMenos3.NetCore.ApiDemo.Database.DbContexts;
using TMenos3.NetCore.ApiDemo.Database.Repositories;

namespace TMenos3.NetCore.ApiDemo.Database.UnitOfWork
{
    internal class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _dbContext;

        public UnitOfWork(AppDbContext dbContext, IDistributedCache cache)
        {
            _dbContext = dbContext;
            // Regular implementations inject the context into repositories.
            // However, by doing that the repository would have more control
            // over the context than it would be required.
            // For instance, calling SaveChangesAsync() is not allowed within
            // repositories
            JobRepository = new CachedJobRepository(new JobRepository(dbContext.Jobs), cache);
            LeagueRepository = new LeagueRepository(dbContext.Leagues);
            TeamRepository = new TeamRepository(dbContext.Teams);
        }

        public IJobRepository JobRepository { get; }

        public ILeagueRepository LeagueRepository { get; }

        public ITeamRepository TeamRepository { get; }

        public Task SaveChangesAsync() =>
            _dbContext.SaveChangesAsync();
    }
}
