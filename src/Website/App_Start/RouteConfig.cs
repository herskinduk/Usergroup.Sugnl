using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Website
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            //routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            Sitecore.Diagnostics.Log.Debug("---> ROUTING");

            routes.MapRoute(
                name: "Default",
                url: "data/{controller}/{action}/{id}",
                defaults: new { controller = "Wiki", action = "Run", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Search",
                url: "test/search/{radius}",
                defaults: new { controller = "search", action = "search", radius = 100 }
            );
        }
    }
}
