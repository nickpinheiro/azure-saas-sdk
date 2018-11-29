using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Saas.Logic.Orchestration.Api.Models
{
    public class Tenant
    {
        [Display(Name = "Company Name")]
        [Required]
        [JsonProperty(Required = Required.Always, PropertyName = "name")]
        public string Name { get; set; }
        //[JsonProperty(PropertyName = "type")]
        //public string Type { get; set; }
        //[JsonProperty(PropertyName = "category")]
        //public string Category { get; set; }
        //[JsonProperty(PropertyName = "region")]
        //public string Region { get; set; }
        [JsonProperty(PropertyName = "productId")]
        public int ProductId { get; set; }
    }
}