using Owin;


namespace LPush.WebApi
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseNancy(new Nancy.Owin.NancyOptions()
            {

            });
        }
    }
}

