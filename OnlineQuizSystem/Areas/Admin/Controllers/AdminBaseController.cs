using DAL.Entities;
using DAL.Services;
using OnlineQuizSystem.CommonCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace OnlineQuizSystem.Areas.Admin.Controllers
{
    public class AdminBaseController : Controller
    {

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //--if teacher session null, then redirect to admin login page
            if (SessionManager.user == null)
            {

                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                    {
                        controller = "AdminAccount",
                        action = "Login",
                        area="Admin"
                }));
              
            }



            base.OnActionExecuting(filterContext);
        }
    }
}