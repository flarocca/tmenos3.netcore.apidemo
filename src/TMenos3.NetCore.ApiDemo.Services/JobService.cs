using AutoMapper;
using System;
using System.Net;
using System.Threading.Tasks;
using TMenos3.NetCore.ApiDemo.Contracts.Commands;
using TMenos3.NetCore.ApiDemo.Database.UnitOfWork;
using TMenos3.NetCore.ApiDemo.Infrastructure.EventBus.Abstractions;
using TMenos3.NetCore.ApiDemo.Models;

namespace TMenos3.NetCore.ApiDemo.Services
{
    public class JobService : IJobService
    {
        private readonly IEventBus _eventBus;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public JobService(IEventBus eventBus, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _eventBus = eventBus;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Guid> CreateAsync(Job job)
        {
            job.Id = Guid.NewGuid();
            job.StatusCode = HttpStatusCode.Accepted;
            var entityJob = _mapper.Map<Database.Entities.Job>(job);
            _unitOfWork.JobRepository.Add(entityJob);
            await _unitOfWork.SaveChangesAsync();

            var command = new ImportLeagueCommand(job.Id, job.LeagueCode, job.CallbackUri);
            await _eventBus.PublishAsync(command);

            return job.Id;
        }

        public async Task<Job> GetAsync(Guid jobId)
        {
            var entityJob = await _unitOfWork.JobRepository.GetAsync(jobId);
            return _mapper.Map<Job>(entityJob);
        }
    }
}
