using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using TMenos3.NetCore.ApiDemo.Database.DbContexts;
using TMenos3.NetCore.ApiDemo.Database.Repositories;
using TMenos3.NetCore.ApiDemo.IntegartionTests.Utils;
using Xunit;
using FluentAssertions;

namespace TMenos3.NetCore.ApiDemo.IntegartionTests.Repositories
{
    public class JobRepositoryTests
    {
        [Fact]
        public async Task CanCreateJob()
        {
            // Arrange
            var expectedJob = new Database.Entities.Job
            {
                Id = Guid.NewGuid(),
                LeagueCode = "LC",
                StatusCode = System.Net.HttpStatusCode.Created
            };
            using var executionContext = new ExecutionContext<AppDbContext>(true);
            using (var actionContext = await executionContext.CreateContextAsync())
            {
                var sut = new JobRepository(actionContext.Jobs);

                // Act
                sut.Add(expectedJob);
                await actionContext.SaveChangesAsync();
            }

            // Assert
            using var assertionContext = await executionContext.CreateContextAsync();
            var createdJob = await assertionContext.Jobs
                .FirstOrDefaultAsync(job => job.Id == expectedJob.Id);

            createdJob.Should().NotBeNull();
            createdJob.Should().BeEquivalentTo(expectedJob);
        }
    }
}
