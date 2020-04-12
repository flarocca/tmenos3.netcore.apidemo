namespace TMenos3.NetCore.ApiDemo.Services.Infrastructure
{
    public class FootballDataSettings
    {
        public const string FootballDataSettingsSection = "FootballData";

        public string ApiToken { get; set; }

        public string BaseUrl { get; set; }

        public string TokenHeader { get; set; }
    }
}
