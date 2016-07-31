using Nancy;

namespace LPush.WebApi.Infrastructure
{
    public static class NancyModuleExtensions
    {
        public static dynamic Response<TModle>(this NancyModule module, TModle model)
        {
#if DEBUG
            return module.Response.AsJson(model);
#else
            return model;           
#endif

        }
    }
}