using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using TMenos3.NetCore.ApiDemo.Services.InternalModels;
using TMenos3.NetCore.ApiDemo.Services.Utils;

namespace TMenos3.NetCore.ApiDemo.Services
{
    internal class FootballDataService : IFootballDataService
    {
        private readonly HttpClient _httpClient;

        public FootballDataService(HttpClient httpClient) =>
            _httpClient = httpClient;

        public async Task<Competition> GetFullCompetitionAsync(string competitionCode)
        {
            using var response = await _httpClient.GetAsync(
                $"competitions/{competitionCode}/matches",
                HttpCompletionOption.ResponseHeadersRead);

            if(response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }

            response.EnsureSuccessStatusCode();

            using var stream = await response.Content.ReadAsStreamAsync();
            var json = stream.DeserializeJson<JObject>();

            var competition = json.SelectToken("competition").ToObject<Competition>();
            competition.Teams = json.SelectToken("matches")
                .ToObject<IEnumerable<Match>>()
                .SelectMany(match => new int[] { match.AwayTeam.Id, match.HomeTeam.Id });

            return competition;
        }

        public async Task<Team> GetTeamAsync(int teamId)
        {
            var response = await _httpClient.GetAsync(
                $"teams/{teamId}",
                HttpCompletionOption.ResponseHeadersRead);

            using var stream = await response.Content.ReadAsStreamAsync();
            var x = stream.DeserializeJson<Team>();

            return x;
        }
    }
}
