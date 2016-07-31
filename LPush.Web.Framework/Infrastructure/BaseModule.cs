using Nancy;

namespace LPush.Web.Framework
{
    public class BaseModule: NancyModule
    {
        public BaseModule() :base()
        {

        }


        public BaseModule(string modulePath) :base(modulePath){

        }
    }
}