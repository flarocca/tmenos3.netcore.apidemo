using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;
using TMenos3.NetCore.ApiDemo.Contracts.Dtos.Requests;
using TMenos3.NetCore.ApiDemo.Contracts.Dtos.Responses;
using TMenos3.NetCore.ApiDemo.Services;
using TMenos3.NetCore.ApiDemo.Services.Exceptions;

namespace TMenos3.NetCore.ApiDemo.API.Controllers
{
    [Route("import-league")]
    [ApiController]
    public class LeaguesController : ControllerBase
    {
        private readonly ILeaguesService _leaguesService;
        private readonly IMapper _mapper;

        public LeaguesController(ILeaguesService leaguesService, IMapper mapper)
        {
            _leaguesService = leaguesService;
            _mapper = mapper;
        }

        [HttpGet("{leagueCode}", Name = nameof(GetLeagueAsync))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetLeagueAsync([FromRoute] LeaguesRequest request)
        {
            try
            {
                var league = await _leaguesService.GetLeagueAsync(request.LeagueCode);
                var response = _mapper.Map<LeagueResponse>(league);

                return Ok(response);
            }
            catch (LeagueDoesNotExistException)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// This endpoint is obsolete and should be used in a production
        /// environment. This operation can be potentially large and
        /// keeping the client waiting blocking the thread should not be
        /// an option.
        /// This endpoint should enqueue the request and return immediatelly 
        /// with a Accepted + location header.
        /// For further example look at POST /jobs/import-league
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("{leagueCode}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status504GatewayTimeout)]
        public async Task<IActionResult> ImportAsync([FromRoute] LeaguesRequest request)
        {
            try
            {
                await _leaguesService.ImportAsync(request.LeagueCode);

                return CreatedAtRoute(
                    nameof(GetLeagueAsync), 
                    new { leagueCode = request.LeagueCode }, 
                    null);
            }
            catch (LeagueDoesNotExistException)
            {
                return NotFound();
            }
            catch (LeagueAlreadyImportedException)
            {
                return Conflict();
            }
            catch (ServiceTimeoutException)
            {
                return new ObjectResult(request.LeagueCode)
                {
                    StatusCode = (int)HttpStatusCode.GatewayTimeout
                };
            }
        }
    }
}