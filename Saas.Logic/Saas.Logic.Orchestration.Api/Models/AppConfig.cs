using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Saas.Logic.Orchestration.Api.Models
{
    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public class AppConfig
    {
        public AppSettings Properties { get; set; }
    }

    public class AppSettings
    {
        [JsonProperty]
        public string BlobPath { get; set; }
        [JsonProperty]
        public string ConnectionTimeOut { get; set; }
        [JsonProperty]
        public string DatabasePassword { get; set; }
        [JsonProperty]
        public string DatabaseServerPort { get; set; }
        [JsonProperty]
        public string DatabaseUser { get; set; }
        [JsonProperty]
        public string ResetEventDates { get; set; }
        [JsonProperty]
        public string ServicePlan { get; set; }
        [JsonProperty]
        public string SqlProtocol { get; set; }
        [JsonProperty]
        public string TenantServer { get; set; }
        [JsonProperty]
        public string TenantDatabase { get; set; }
        [JsonProperty]
        public string LearnHowFooterUrl { get; set; }
        [JsonProperty]
        public string ASPNETCORE_ENVIRONMENT { get; set; }
        [JsonProperty]
        public string WEBSITE_NODE_DEFAULT_VERSION { get; set; }
        [JsonProperty]
        public string DefaultRequestCulture { get; set; }
    }
}