using System.Web.Mvc;

namespace OnlineQuizSystem.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {


            context.MapRoute(
               "Admin_Quiz_categories",
               "admin/quiz/categories/{pageId}",
               new { controller = "AdminBasicData", action = "QuizCategories", pageId = UrlParameter.Optional },
               new[] { "OnlineQuizSystem.Areas.Admin.Controllers" }
           );

            context.MapRoute(
                  "Admin_Home",
                  "admin/home",
                  new { controller = "AdminHome", action = "Dashboard" },
                  new[] { "OnlineQuizSystem.Areas.Admin.Controllers" }
              );

            context.MapRoute(
                "Admin_Students_List",
                "admin/students-list/{pageId}",
                new { controller = "AdminHome", action = "StudentsList" , pageId = UrlParameter.Optional },
                new[] { "OnlineQuizSystem.Areas.Admin.Controllers" }
            );

            context.MapRoute(
               "Admin_Teachers_List",
               "admin/teachers-list/{pageId}",
               new { controller = "AdminHome", action = "TeachersList", pageId = UrlParameter.Optional },
               new[] { "OnlineQuizSystem.Areas.Admin.Controllers" }
           );

            context.MapRoute(
                "Admin_Students_Courses_List",
                "admin/students-courses/{student_id}/{pageId}",
                new { controller = "AdminHome", action = "StudentRegisteredCourses", student_id = UrlParameter.Optional, pageId = UrlParameter.Optional },
                new[] { "OnlineQuizSystem.Areas.Admin.Controllers" }
            );

            context.MapRoute(
              "Admin_Teachers_Assign_Courses",
              "admin/teacher-courses/{user_id}/{pageId}",
              new { controller = "AdminHome", action = "TeacherAssignCourses", user_id = UrlParameter.Optional, pageId = UrlParameter.Optional },
              new[] { "OnlineQuizSystem.Areas.Admin.Controllers" }
          );

            context.MapRoute(
            "Admin_Courses_List",
              "admin/courses/{pageId}",
            new { controller = "AdminHome", action = "Courses", pageId = UrlParameter.Optional },
            new[] { "OnlineQuizSystem.Areas.Admin.Controllers" }
        );

            context.MapRoute(
           "Admin_Course_Categories",
             "admin/course/categories/{pageId}",
           new { controller = "AdminBasicData", action = "CourseCategories", pageId = UrlParameter.Optional },
           new[] { "OnlineQuizSystem.Areas.Admin.Controllers" }
       );
            context.MapRoute(
            "Admin_Login",
              "account/login",
            new { controller = "AdminAccount", action = "Login" },
            new[] { "OnlineQuizSystem.Areas.Admin.Controllers" }

        );

         context.MapRoute(
        "DocumentationPath",
         "documentation/index",
        new { controller = "Documentation", action = "Index" },
        new[] { "OnlineQuizSystem.Areas.Admin.Controllers" }

        );



            //--default rout
            context.MapRoute(
                "Admin_default",
                "Admin/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}