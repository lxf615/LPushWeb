using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(LPush.Web.Startup))]
namespace LPush.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
