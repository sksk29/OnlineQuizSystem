using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace OnlineQuizSystem.CommonCode
{
    public class StudentAuthorization : ActionFilterAttribute
    {
       
       
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            //--if teacher session null, then redirect to admin login page
            if (SessionManager.student == null)
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                {
                    controller = "AdminAccount",
                    action = "Login",
                    area = "Admin"
                }));

            }
        

            base.OnActionExecuting(filterContext);
        }

    }
}