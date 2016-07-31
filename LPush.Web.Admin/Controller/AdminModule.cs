using LPush.Web.Framework;

namespace LPush.Web.Admin.Controller
{
    public class AdminModule : BaseModule
    {
#if MergeSite
        public AdminModule() : base("Admin")
        {
        }

        public AdminModule(string moduleName) : base("Admin/" + moduleName)
        {
        }
#else
        public AdminModule() : base()
        {
        }

        public AdminModule(string moduleName) : base(moduleName)
        {
        }
#endif
    }
}