using AutoMapper;
using FakeItEasy;
using System;
using System.Threading.Tasks;
using TMenos3.NetCore.ApiDemo.Contracts.Commands;
using TMenos3.NetCore.ApiDemo.Database.Repositories;
using TMenos3.NetCore.ApiDemo.Database.UnitOfWork;
using TMenos3.NetCore.ApiDemo.Infrastructure.EventBus.Abstractions;
using TMenos3.NetCore.ApiDemo.Services;
using Xunit;

namespace TMenos3.NetCore.ApiDemo.UnitTests.Services
{
    public class JobServiceTests
    {
        [Fact]
        public async Task CanCreateJob()
        {
            // Arrange
            var expectedJob = new Models.Job
            {
                LeagueCode = "PL"
            };
            var entityJob = new Database.Entities.Job
            {
                LeagueCode = "PL",
                StatusCode = System.Net.HttpStatusCode.Created
            };
            var eventBus = A.Fake<IEventBus>();
            var jobRepository = A.Fake<IJobRepository>();
            var unitOfWork = A.Fake<IUnitOfWork>();
            var mapper = A.Fake<IMapper>();

            A.CallTo(() => unitOfWork.JobRepository)
                .Returns(jobRepository);
            A.CallTo(() => mapper.Map<Database.Entities.Job>(A<Models.Job>.Ignored))
                .Returns(entityJob);

            var sut = new JobService(eventBus, unitOfWork, mapper);

            // Act
            await sut.CreateAsync(expectedJob);

            // Assert
            A.CallTo(() => mapper.Map<Database.Entities.Job>(A<Models.Job>.That.Matches(
                modelJob => modelJob.Id != Guid.Empty &&
                            modelJob.StatusCode == System.Net.HttpStatusCode.Accepted &&
                            modelJob.LeagueCode == expectedJob.LeagueCode &&
                            string.IsNullOrWhiteSpace(modelJob.CallbackUri))))
                .MustHaveHappened();

            A.CallTo(() => jobRepository.Add(entityJob))
                .MustHaveHappened();

            A.CallTo(() => eventBus.PublishAsync(A<ImportLeagueCommand>.That.Matches(
                command => command.Id != Guid.Empty &&
                           command.LeagueCode == expectedJob.LeagueCode &&
                           string.IsNullOrWhiteSpace(command.CallbackUri))))
                .MustHaveHappened();
        }
    }
}
