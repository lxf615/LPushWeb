using Nancy;

namespace LPush.WebApi
{
    /// <summary>
    /// 继承NancyModule，控制访问路径为Api/....
    /// </summary>
    public class ApiModule : NancyModule
    {
        public ApiModule() : base("Api")
        {

        }

        public ApiModule(string modulePath):base("Api/"+modulePath)
        {

        }
    }
}
