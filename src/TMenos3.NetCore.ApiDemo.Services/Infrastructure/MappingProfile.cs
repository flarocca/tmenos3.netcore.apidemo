using AutoMapper;
using System.Linq;

namespace TMenos3.NetCore.ApiDemo.Services.Infrastructure
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<InternalModels.Competition, Database.Entities.League>()
                .ForMember(d => d.AreaName, opt => opt.MapFrom(d => d.Area.Name))
                .ForMember(d => d.LeagueTeams, opt => opt.Ignore());

            CreateMap<InternalModels.Team, Database.Entities.Team>()
                .ForMember(d => d.AreaName, opt => opt.MapFrom(d => d.Area.Name))
                .ForMember(d => d.Players, opt => opt.MapFrom(d => d.Squad))
                .ForMember(d => d.LeagueTeams, opt => opt.Ignore());

            CreateMap<InternalModels.Player, Database.Entities.Player>();

            CreateMap<Database.Entities.League, Models.League>()
                .ForMember(d => d.Teams, opt => opt.MapFrom(
                    d => d.LeagueTeams.Select(leagueTeam => leagueTeam.Team)));

            CreateMap<Database.Entities.Team, Models.Team>();

            CreateMap<Database.Entities.Player, Models.Player>();

            CreateMap<Models.Job, Database.Entities.Job>()
                .ReverseMap();
        }
    }
}
