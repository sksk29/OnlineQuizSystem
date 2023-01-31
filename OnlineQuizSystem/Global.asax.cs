using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace OnlineQuizSystem
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //--Following line is only added for making this site to be open in iframe of Codecanyon.net website. Make it enable if you want to open from iframe
           // System.Web.Helpers.AntiForgeryConfig.SuppressXFrameOptionsHeader = true;
        }


       
    }
}
