using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Saas.Logic.Orchestration.Api.Models
{
    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public class WebAppConfig
    {
        public Settings Properties { get; set; }
    }

    public class Settings
    {
        [JsonProperty(PropertyName = "alwaysOn")]
        public bool AlwaysOn { get; set; }
    }
}