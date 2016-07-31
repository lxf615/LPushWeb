using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Owin;
using Autofac;
namespace LPush.Web.Framework.Middleware
{
    public static class AppBuilderExtensions
    {
        public static IAppBuilder UseTokenAuthorize(this IAppBuilder app, AuthorizeOptions options = null)
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<AuthorizeMiddleware>();
            options = options ?? new AuthorizeOptions()
            {
                
            };
            builder.RegisterInstance(options);
            
            var container = builder.Build();
            app.UseAutofacMiddleware(container);

            return app;
        }
    }
}
