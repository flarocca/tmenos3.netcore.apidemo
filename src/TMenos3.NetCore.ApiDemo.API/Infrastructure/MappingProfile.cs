using AutoMapper;
using TMenos3.NetCore.ApiDemo.Contracts.Dtos.Requests;
using TMenos3.NetCore.ApiDemo.Contracts.Dtos.Responses;

namespace TMenos3.NetCore.ApiDemo.API.Infrastructure
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Models.League, LeagueResponse>();

            CreateMap<Models.Team, TeamResponse>();

            CreateMap<Models.Player, PlayerResponse>();

            CreateMap<JobRequest, Models.Job>();
        }
    }
}
