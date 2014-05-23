using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

[assembly: WebActivatorEx.PreApplicationStartMethod(
typeof(Website.App_Start.Bootstrap), "PreStart")]

[assembly: WebActivatorEx.PostApplicationStartMethod(
    typeof(Website.App_Start.Bootstrap), "PostStart")]

namespace Website.App_Start
{

    public class Bootstrap
    {

        public static void PreStart()
        {
            Sitecore.Diagnostics.Log.Info("Bootstrap: Performing PreStart actions", "MvcContribPackage");
            
        }

        public static void PostStart()
        {
            Sitecore.Diagnostics.Log.Info("Bootstrap: Performing PostStart actions", "MvcContribPackage");


            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

        }
    }
}