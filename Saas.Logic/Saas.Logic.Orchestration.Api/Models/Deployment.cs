using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Saas.Logic.Orchestration.Api.Models
{
    public class Deployment
    {
        public string id { get; set; }
        public string name { get; set; }
        public DeploymentProperties properties { get; set; }
    }

    public class DeploymentProperties
    {
        public Templatelink templateLink { get; set; }
        public string templateHash { get; set; }
        public Parameters parameters { get; set; }
        public string mode { get; set; }
        public string provisioningState { get; set; }
        public DateTime timestamp { get; set; }
        public string duration { get; set; }
        public string correlationId { get; set; }
        public Provider[] providers { get; set; }
        public Dependency[] dependencies { get; set; }
        public Outputs outputs { get; set; }
        public Outputresource[] outputResources { get; set; }
    }

    public class Templatelink
    {
        public string uri { get; set; }
        public string contentVersion { get; set; }
    }

    public class Parameters
    {
        public MaDatabasename madatabaseName { get; set; }
    }

    public class MaDatabasename
    {
        public string type { get; set; }
        public string value { get; set; }
    }

    public class Outputs
    {
    }

    public class Provider
    {
        public string _namespace { get; set; }
        public Resourcetype[] resourceTypes { get; set; }
    }

    public class Resourcetype
    {
        public string resourceType { get; set; }
        public string[] locations { get; set; }
    }

    public class Dependency
    {
        public Dependson[] dependsOn { get; set; }
        public string id { get; set; }
        public string resourceType { get; set; }
        public string resourceName { get; set; }
    }

    public class Dependson
    {
        public string id { get; set; }
        public string resourceType { get; set; }
        public string resourceName { get; set; }
    }

    public class Outputresource
    {
        public string id { get; set; }
    }
}