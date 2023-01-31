using System.Web.Mvc;

namespace OnlineQuizSystem.Areas.Users
{
    public class UsersAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Users";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                 "Student_Login",
                 "Users/UserAccount/StudentLogin",
                 new { controller = "UserAccount", action = "StudentLogin" },
                 new[] { "OnlineQuizSystem.Areas.Users.Controllers" }
             );

            context.MapRoute(
                "StudentQuizList",
                "users/home/quiz-list/{course_id}/{pageId}",
                new { controller = "UserHome", action = "StudentQuizList" },
                new[] { "OnlineQuizSystem.Areas.Users.Controllers" }
            );

            context.MapRoute(
              "Student_Assignment_List",
              "users/home/assignment-list/{course_id}/{pageId}",
              new { controller = "UserHome", action = "StudentAssignmentList" },
              new[] { "OnlineQuizSystem.Areas.Users.Controllers" }
          );

            context.MapRoute(
             "Student_Home_Page",
             "user/home",
             new { controller = "UserHome", action = "Index" },
             new[] { "OnlineQuizSystem.Areas.Users.Controllers" }
         );

            context.MapRoute(
          "Student_Courses_List",
            "users/courses/{pageId}",
          new { controller = "UserHome", action = "CoursesList", pageId = UrlParameter.Optional },
          new[] { "OnlineQuizSystem.Areas.Users.Controllers" }
         );

            context.MapRoute(
               "Student_Start_Quiz",
               "users/home/start-quiz/{quiz_ID}",
               new { controller = "UserHome", action = "StartQuiz" },
               new[] { "OnlineQuizSystem.Areas.Users.Controllers" }
           );


            context.MapRoute(
             "Student_Start_Assignment",
             "users/home/start-assignment/{assignment_id}",
             new { controller = "UserHome", action = "StartAssignment" , assignment_id = UrlParameter.Optional },
             new[] { "OnlineQuizSystem.Areas.Users.Controllers" }
         );

            context.MapRoute(
                "Users_default",
                "Users/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}