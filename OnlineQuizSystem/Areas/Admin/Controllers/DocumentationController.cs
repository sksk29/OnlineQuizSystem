using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineQuizSystem.Areas.Admin.Controllers
{
    public class DocumentationController : Controller
    {
        // GET: Admin/Documentation
        public ActionResult Index()
        {
            return View();
        }
    }
}