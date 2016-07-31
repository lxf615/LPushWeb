using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

using LPush.Web.Framework.Middleware;
using LPush.Web.Framework.Filter;
namespace LPush.WebApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config, AuthorizeOptions options)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Filters.Add(new ExceptionFilter(options));
            config.Filters.Add(new ResponseFilter(options));
        }
    }
}
