using Newtonsoft.Json;
using System;
using TMenos3.NetCore.ApiDemo.Infrastructure.EventBus.Messages;

namespace TMenos3.NetCore.ApiDemo.Contracts.Commands
{
    public class ImportLeagueCommand : IEvent
    {
        public Guid Id { get; }

        public string LeagueCode { get; }

        public string CallbackUri { get; }

        [JsonConstructor]
        public ImportLeagueCommand(Guid id, string leagueCode, string callbackUri)
        {
            Id = id;
            LeagueCode = leagueCode;
            CallbackUri = callbackUri;
        }
    }
}
