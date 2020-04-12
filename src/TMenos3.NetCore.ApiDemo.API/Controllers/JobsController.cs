using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading.Tasks;
using TMenos3.NetCore.ApiDemo.Contracts.Dtos.Requests;
using TMenos3.NetCore.ApiDemo.Models;
using TMenos3.NetCore.ApiDemo.Services;

namespace TMenos3.NetCore.ApiDemo.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class JobsController : ControllerBase
    {
        private readonly IJobService _jobService;
        private readonly IMapper _mapper;

        public JobsController(IJobService jobService, IMapper mapper)
        {
            _jobService = jobService;
            _mapper = mapper;
        }

        [HttpGet("import-league/{jobId}", Name = nameof(GetJobAsync))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status504GatewayTimeout)]
        public async Task<IActionResult> GetJobAsync([FromRoute] Guid jobId)
        {
            var job = await _jobService.GetAsync(jobId);

            if (job == null)
            {
                return NotFound();
            }

            return job.StatusCode switch
            {
                HttpStatusCode.Created => CreatedAtRoute(
                                       nameof(LeaguesController.GetLeagueAsync),
                                       new { leagueCode = job.LeagueCode },
                                       null),
                HttpStatusCode.Conflict => Conflict(),
                HttpStatusCode.GatewayTimeout => new ObjectResult(job.LeagueCode)
                {
                    StatusCode = (int)HttpStatusCode.GatewayTimeout
                },
                _ => new ObjectResult(job.LeagueCode)
                {
                    StatusCode = (int)HttpStatusCode.Accepted
                },
            };
        }

        [HttpPost("import-league")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        public async Task<IActionResult> ImportAsync([FromBody] JobRequest request)
        {
            var job = _mapper.Map<Job>(request);

            var jobId = await _jobService.CreateAsync(job);

            return AcceptedAtRoute(
                nameof(GetJobAsync),
                new { jobId },
                null);
        }
    }
}