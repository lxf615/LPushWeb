#if !MergeSite
using Owin;

namespace LPush.Web.Admin
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseNancy();
            //need when host in iis
            //app.UseStageMarker(PipelineStage.MapHandler);
        }
    }
}
#endif

