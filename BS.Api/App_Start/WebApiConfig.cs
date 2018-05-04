using BS.Api.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace BS.Api
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "DefaultApiWithAction",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            //Register handlers
            config.MessageHandlers.Add(new ApiLogHandler());
            config.MessageHandlers.Add(new AuthorizationHeaderHandler());
            //config.MessageHandlers.Add(new EncryptHandler());
        }
    }
}
