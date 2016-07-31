using System;

using Nancy;
using Nancy.Responses;
using Nancy.Bootstrapper;
using Nancy.TinyIoc;
using Nancy.Security;
using Nancy.Authentication.Forms;
using Nancy.Session;
using Nancy.Bootstrappers.Autofac;
using Autofac;

using LPush.Web.Models;
using LPush.Web.Framework;

namespace LPush.Web
{
    public class Bootstrapper : AutofacNancyBootstrapper
    {

        protected override void ConfigureApplicationContainer(ILifetimeScope container)
        {
            var builder = new ContainerBuilder();

            // Here we register our user mapper as a per-request singleton.
            // As this is now per-request we could inject a request scoped
            // database "context" or other request scoped services.

            //Form auth
            builder.RegisterType<UserMapper>().As<IUserMapper>().InstancePerLifetimeScope();
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

            // At request startup we modify the request pipelines to
            // include forms authentication - passing in our now request
            // scoped user name mapper.
            //
            // The pipelines passed in here are specific to this request,
            // so we can add/remove/update items in them as we please.
            var formsAuthConfiguration =
                new FormsAuthenticationConfiguration()
                {
                    RedirectUrl = "~/account/login",
                    UserMapper = container.Resolve<IUserMapper>(),
                };

            FormsAuthentication.Enable(pipelines, formsAuthConfiguration);

            //启用Session
            CookieBasedSessions.Enable(pipelines);
        }
    }
}