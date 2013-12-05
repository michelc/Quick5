using System;
using System.Web.Mvc;
using System.Web.Routing;
using Quick5.Models;
using StackExchange.Profiling;

namespace Quick5
{
    // Remarque : pour obtenir des instructions sur l'activation du mode classique IIS6 ou IIS7, 
    // visitez http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        public static bool IsDbProduction { get; set; }
        public static bool IsDbTests { get; set; }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            MappingConfig.RegisterMappings();
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            MiniProfiler.Start();
        }

        protected void Application_EndRequest(object sender, EventArgs e)
        {
            MiniProfiler.Stop();
        }
    }
}