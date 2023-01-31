using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace OnlineQuizSystem
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

          

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

           
            //routes.MapRoute( 
            //    name: "Admin_QuizCategories",
            //    url: "Admin/{controller}/{action}/{pageId}", 
            //    defaults: new
            //    {
            //        controller = "AdminBasicData",
            //        action = "QuizCategories",
            //        pageId = UrlParameter.Optional
            //    }, namespaces: new[] {
            //    "OnlineQuizSystem.Areas.Admin.Controllers"
            //});

           

        }
    }
}
