using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMenos3.NetCore.ApiDemo.Database.UnitOfWork;
using TMenos3.NetCore.ApiDemo.Services.Exceptions;
using TMenos3.NetCore.ApiDemo.Services.Helpers;
using TMenos3.NetCore.ApiDemo.Services.InternalModels;
using League = TMenos3.NetCore.ApiDemo.Database.Entities.League;

namespace TMenos3.NetCore.ApiDemo.Services
{
    internal class LeaguesService : ILeaguesService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFootballDataService _footballDataService;
        private readonly IBatchFactory _batchFactory;
        private readonly IMapper _mapper;

        public LeaguesService(
            IUnitOfWork unitOfWork,
            IFootballDataService footballDataService,
            IBatchFactory batchFactory,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _footballDataService = footballDataService;
            _batchFactory = batchFactory;
            _mapper = mapper;
        }

        public async Task<Models.League> GetLeagueAsync(string leagueCode)
        {
            var entity = await _unitOfWork.LeagueRepository.GetByCodeAsync(leagueCode);

            if (entity == null)
            {
                throw new LeagueDoesNotExistException();
            }

            return _mapper.Map<Models.League>(entity);
        }

        public async Task ImportAsync(string leagueCode)
        {
            if (await _unitOfWork.LeagueRepository.ExistsByCodeAsync(leagueCode))
            {
                throw new LeagueAlreadyImportedException();
            }

            try
            {
                Competition competition = await ImportCompetitionAsync(leagueCode);
                await ImportTeamsAsync(competition);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (OperationCanceledException)
            {
                throw new ServiceTimeoutException();
            }
        }

        private async Task<Competition> ImportCompetitionAsync(string leagueCode)
        {
            var competition = await _footballDataService.GetFullCompetitionAsync(leagueCode);

            if (competition == null)
            {
                throw new LeagueDoesNotExistException();
            }

            var entityLeague = _mapper.Map<League>(competition);
            _unitOfWork.LeagueRepository.Add(entityLeague);

            return competition;
        }

        private async Task ImportTeamsAsync(Competition competition)
        {
            var teams = await GetFullTeamsAsync(competition);
            var entityTeams = _mapper.Map<IEnumerable<Database.Entities.Team>>(teams);

            foreach (var team in entityTeams)
            {
                if (team.LeagueTeams == null)
                {
                    team.LeagueTeams = new List<Database.Entities.LeagueTeam>();
                }

                team.LeagueTeams.Add(new Database.Entities.LeagueTeam
                {
                    LeagueId = competition.Id,
                    TeamId = team.Id
                });
            }

            _unitOfWork.TeamRepository.AddRange(entityTeams);
        }

        private Task<IEnumerable<Team>> GetFullTeamsAsync(Competition competition) =>
            _batchFactory
                .Create<int, Team>(10)
                .ForList(competition.Teams.Distinct())
                .Apply(teamId => _footballDataService.GetTeamAsync(teamId))
                .ExecuteAsync();
    }
}
