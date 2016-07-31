using Nancy;
using Nancy.Bootstrapper;
using Nancy.Security;
using Nancy.Bootstrappers.Autofac;
using Nancy.Authentication.Token;
using Autofac;

using LPush.Web.Framework;

namespace LPush.Web
{
    public class Bootstrapper : AutofacNancyBootstrapper
    {

        protected override void ConfigureApplicationContainer(ILifetimeScope container)
        {
            var builder = new ContainerBuilder();
            
            //Token auth
            builder.RegisterType<Tokenizer>().As<ITokenizer>().InstancePerLifetimeScope();
            //Web
            var registar = new DependencyRegistrar();
            registar.Register(builder);

            builder.Update(container.ComponentRegistry);
        }

        
  
        protected override void ConfigureRequestContainer(ILifetimeScope container, NancyContext context)
        {
            base.ConfigureRequestContainer(container, context);
        }



        protected override void ApplicationStartup(ILifetimeScope container, IPipelines pipelines)
        {
            base.ApplicationStartup(container, pipelines);

            StaticConfiguration.EnableRequestTracing = true;
            //显示详细错误信息
            StaticConfiguration.DisableErrorTraces = false;
            //启用AntiToken
            Csrf.Enable(pipelines);
        }
 

        protected override void RequestStartup(ILifetimeScope container, IPipelines pipelines, NancyContext context)
        {
            base.RequestStartup(container, pipelines, context);

            TokenAuthentication.Enable(pipelines, new TokenAuthenticationConfiguration(container.Resolve<ITokenizer>()));
        }
    }
}