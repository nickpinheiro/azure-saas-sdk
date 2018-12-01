using Microsoft.Rest;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Saas.Presentation.Provider.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpGet]
        public ActionResult Tenant(int id, string planName)
        {
            ViewBag.Message = "Your tenant page.";
            ViewBag.Name = planName;

            return View();
        }

        [HttpPost]
        public ActionResult Tenant(Models.Tenant tenant)
        {
            tenant.ProductId = 3;

            ContosoDevApiOrchestration client = new ContosoDevApiOrchestration(new Uri(ConfigurationManager.AppSettings["SaaSOrchestrationApiBaseUri"]), new AnonymousCredential());
            client.Accepted(tenant);

            return View("Confirmation");
        }

        public ActionResult Confirmation()
        {
            ViewBag.Message = "Your confirmation page.";

            return View();
        }

        private class AnonymousCredential : ServiceClientCredentials
        {

        }
    }
}