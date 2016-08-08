using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

using Microsoft.Owin;
using Owin;

using LPush.Web.Framework.Middleware;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]
[assembly: OwinStartup(typeof(LPush.WebApi.Startup))]
namespace LPush.WebApi
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            log4net.Config.BasicConfigurator.Configure();

            //authorize
            AuthorizeOptions options = new AuthorizeOptions()
            {
                ConfigPath = HttpContext.Current.Server.MapPath("~/Config.ini")
            };
            app.UseTokenAuthorize(options);

            //webapi
            HttpConfiguration config = new HttpConfiguration();
            WebApiConfig.Register(config, options);
            app.UseWebApi(config);
        }
    }
}
