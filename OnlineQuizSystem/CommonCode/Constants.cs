using DAL.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace OnlineQuizSystem.CommonCode
{
    public static class Constants
    {

      

        public static int  ITEMS_PER_PAGE
        {
            get
            {
                try
                {
                    int pageSize= Convert.ToInt32(ConfigurationManager.AppSettings["ITEMS_PER_PAGE"]);
                    if (pageSize==0 || pageSize<1)
                    {
                        return 10;
                    }
                    else
                    {
                        return pageSize;
                    }
                }
                catch(Exception ex)
                {
                    AdminServices.Instance.LogErrorInDatabase(ex.Message, "Items per page error");
                    return 10;
                }
               
            }
        }
        
        
        //ITEMS_PER_PAGE

        public static string Admin_Home_Page
        {
            get
            {
                return ConfigurationManager.AppSettings["Admin_HomePage_URL"];
            }
        }

        public static string Student_Home_Page
        {
            get
            {
                return ConfigurationManager.AppSettings["Student_HomePage_URL"];
            }
        }

        public static string Teacher_Home_Page
        {
            get
            {
                return ConfigurationManager.AppSettings["Teacher_HomePage_URL"];
            }
        }


    }
}