using Nancy;
using Nancy.ModelBinding;
using Nancy.Security;

using LPush.Core.Caching;

using LPush.Model;
using LPush.Web.Framework;
using LPush.Web.Admin.Models;

namespace LPush.Web.Admin.Controller
{
    public class HomeController : AdminModule
    {
        private ICacheManager cacheManager = null;

        public HomeController(ICacheManager cacheManager) 
        {
            this.RequiresAuthentication();

            this.cacheManager = cacheManager;
            Get["/"] = _ => Index();
            Get["/Index"] = _ => Index();
        }

        private dynamic Index()
        {
            this.CreateNewCsrfToken();

            this.ViewBag.Title = "凌风推送";
            return View["Home/Index"];
        }
    }
}