using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;



namespace OnlineQuizSystem.CommonCode
{
    public static class SessionManager
    {


        public static users user
        {
            get
            {
                users user = new users();
                if (System.Web.HttpContext.Current.Session["User"] != null)
                {
                    user = System.Web.HttpContext.Current.Session["User"] as users;
                }
                else
                {
                    user = null;
                }
                return user;
            }
        }

        public static users teacher
        {
            get
            {
                users user = new users();
                if (System.Web.HttpContext.Current.Session["Teacher"] != null)
                {
                    user = System.Web.HttpContext.Current.Session["Teacher"] as users;
                }
                else
                {
                    user = null;
                }
                return user;
            }
        }

        public static students student
        {
            get
            {
                students std = new students();
                if (System.Web.HttpContext.Current.Session["Student"] != null)
                {
                    std = System.Web.HttpContext.Current.Session["Student"] as students;
                }
                else
                {
                    std = null;
                }
                return std;
            }
        }

    }
}