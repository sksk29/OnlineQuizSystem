using DAL.Entities;
using DAL.Services;
using OnlineQuizSystem.CommonCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace OnlineQuizSystem.Areas.Admin.Controllers
{
    public class AdminAccountController : Controller
    {
        // GET: Admin/AdminAccount
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string email, string password, string UserType)
        {

            if (String.IsNullOrEmpty(email) || String.IsNullOrEmpty(password) || String.IsNullOrEmpty(UserType))
            {
                TempData["ErrorMsg"] = "Incorrect email or password!";
                return RedirectToAction("Login", "AdminAccount");
            }

        

            if (UserType=="Admin")
            {
                 var user = AdminServices.Instance.UserLogin(email, password, UserType);
                if (user != null && user.user_id>0)
                {
                    Session.Timeout = 120;
                    Session["User"] = user;
                    return RedirectToAction("Dashboard", "AdminHome");
                }
                else
                {
                    TempData["ErrorMsg"] = "Incorrect email or password!";
                    return RedirectToAction("Login", "AdminAccount");
                }
            }
            else if (UserType == "Teacher")
            {
              var  user = AdminServices.Instance.UserLogin(email, password, UserType);
                if (user != null &&  user.user_id > 0)
                {
                    Session.Timeout = 120;
                    Session["Teacher"] = user;
                    return RedirectToAction("Index", "TeacherHome" , new { area = "Teachers" });
                }
                else
                {
                    TempData["ErrorMsg"] = "Incorrect email or password!";
                    return RedirectToAction("Login", "AdminAccount");
                }
            }
            else if (UserType == "Student")
            {
               var  stndtUser = AdminServices.Instance.StudentLogin(email, password);
                if (stndtUser != null && stndtUser.student_id > 0)
                {
                    Session.Timeout = 120;
                    Session["Student"] = stndtUser;
                    return RedirectToAction("Index", "UserHome", new { area = "Users" });
                }
                else
                {
                    TempData["ErrorMsg"] = "Incorrect email or password!";
                    return RedirectToAction("Login", "AdminAccount");
                }
            }
            else
            {
                TempData["ErrorMsg"] = "Incorrect email or password!";
                return RedirectToAction("Login", "AdminAccount");
            }

          
           


        }

        [HttpGet]
        public ActionResult Logout()
        {

            FormsAuthentication.SignOut();

            Session.Clear();
            Session.Abandon();
            Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));
            Response.Cache.SetExpires(DateTime.Now.AddDays(-1));
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoStore();
            Session["User"] = null;
            Session["Student"] =null;
            Session["Teacher"] = null;


            return RedirectToAction("Login", "AdminAccount");
        }
    }
}