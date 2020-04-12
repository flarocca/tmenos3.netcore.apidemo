using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TMenos3.NetCore.ApiDemo.Contracts.Dtos.Requests;
using TMenos3.NetCore.ApiDemo.Contracts.Dtos.Responses;
using TMenos3.NetCore.ApiDemo.Services;

namespace TMenos3.NetCore.ApiDemo.API.Controllers
{
    [Route("total-players")]
    [ApiController]
    public class PlayersController : ControllerBase
    {
        private readonly IPlayersService _playersService;

        public PlayersController(IPlayersService playersService) =>
            _playersService = playersService;

        [HttpGet("{leagueCode}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCountAsync([FromRoute] PlayersCountRequest request)
        {
            var count = await _playersService.GetTotalPlayersByLeagueAsync(request.LeagueCode);

            return Ok(new PlayersCountResponse { Total = count });
        }
    }
}