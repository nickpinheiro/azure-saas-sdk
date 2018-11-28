using Saas.Logic.Orchestration.Api.Classes;
using Swashbuckle.Swagger.Annotations;
using System.Collections.Generic;
using System.Net;
using System.Web.Hosting;
using System.Web.Http;

namespace Saas.Logic.Orchestration.Api.Controllers
{
    public class TenantController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public string GetById(int id)
        {
            return "value";
        }

        // POST api/<controller>
        [Route("api/tenant")]
        [SwaggerOperation("Accepted")]
        [SwaggerResponse(HttpStatusCode.Accepted)]
        public void Post([FromBody]Models.Tenant tenant)
        {
            HostingEnvironment.QueueBackgroundWorkItem(ct => Tenant.CreateTenantAsync(tenant));
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}
