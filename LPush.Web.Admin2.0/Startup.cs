using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(LPush.Web.Admin.Startup))]
namespace LPush.Web.Admin
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            //IOC
            Bootstrapper.Register();
            
        }
    }
}
