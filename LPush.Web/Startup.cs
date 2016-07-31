using Owin;
using Microsoft.Owin.Extensions;
using Nancy;
using Nancy.Owin;

namespace LPush.Web
{
	public class Startup
	{
		public void Configuration(IAppBuilder app)
		{
            app.UseNancy();
            //need when host in iis
           	//app.UseStageMarker(PipelineStage.MapHandler);
        }
	}
}

