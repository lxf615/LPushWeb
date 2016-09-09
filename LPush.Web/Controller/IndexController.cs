using Nancy;
using Nancy.ModelBinding;
using Nancy.Security;


using LPush.Core.Caching;

using LPush.Web.Framework;
using LPush.Web.Models;

namespace LPush.Web.Controller
{
    public class IndexController : BaseModule
    {
        private readonly ICacheManager cacheManager = null;

        public IndexController(ICacheManager cacheManager)
        {
            //this.RequiresAuthentication();

            this.cacheManager = cacheManager;
            Get["/"] = _ => Index();
            Get["/Index"] = _ => Index();
        }

        private dynamic Index()
        {
            this.ViewBag.Title = "凌风推送";
            var model = new HomeViewModel()
            {
                Name = this.Context.CurrentUser == null ? string.Empty : this.Context.CurrentUser.UserName,
            };
            return View["Home/Index",model];
        }
    }
}