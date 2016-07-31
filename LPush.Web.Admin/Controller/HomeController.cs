using Nancy;
using Nancy.ModelBinding;
using Nancy.Security;

using LPush.Core.Caching;

using LPush.Model.Sample;
using LPush.Web.Framework;
using LPush.Web.Admin.Models;
using LPush.Model;
using LPush.Service.Sample;

namespace LPush.Web.Admin.Controller
{
    public class HomeController : AdminModule
    {
        private ICacheManager cacheManager = null;

        public HomeController(ICacheManager cacheManager, IExampleService exampleService) 
        {
            this.RequiresAuthentication();

            this.cacheManager = cacheManager;
            Get["/"] = _ => Index();
            Get["/Index"] = _ => Index();

            Get["/Example/{Id}"] = _ =>
            {
                var example = exampleService.GetExampleById(_.Id);
                return example;
            };
        }

        private dynamic Index()
        {
            this.CreateNewCsrfToken();

            this.ViewBag.Title = "凌风推送";
            return View["Home/Index"];
        }
    }
}