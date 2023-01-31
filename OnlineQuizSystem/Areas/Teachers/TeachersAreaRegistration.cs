using System.Web.Mvc;

namespace OnlineQuizSystem.Areas.Teachers
{
    public class TeachersAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Teachers";
            }
        }


        public override void RegisterArea(AreaRegistrationContext context)
        {

            context.MapRoute(
               "Teacher_Quiz_list",
                 // "admin/quiz/list/{pageId}/{course_id}",
                 "teacher/quiz/list/{course_id}/{pageId}",
               new { controller = "TeacherHome", action = "QuizList", course_id = UrlParameter.Optional, pageId = UrlParameter.Optional },
               new[] { "OnlineQuizSystem.Areas.Teachers.Controllers" }
           );

            context.MapRoute(
             "Teacher_Assignment_list",
               // "admin/quiz/list/{pageId}/{course_id}",
               "teacher/assignment/list/{course_id}/{pageId}",
             new { controller = "TeacherHome", action = "AssignmentList", course_id = UrlParameter.Optional, pageId = UrlParameter.Optional },
             new[] { "OnlineQuizSystem.Areas.Teachers.Controllers" }
         );

            context.MapRoute(
             "Teacher_Add_Quiz",
               "teacher/add-quiz",
             new { controller = "TeacherHome", action = "AddQuiz" },
             new[] { "OnlineQuizSystem.Areas.Teachers.Controllers" }
         );

            context.MapRoute(
          "Teacher_Add_Assignment",
            "teacher/add-assignment",
          new { controller = "TeacherHome", action = "AddAssignment" },
          new[] { "OnlineQuizSystem.Areas.Teachers.Controllers" }
      );

            context.MapRoute(
         "Teacher_Students_List",
           "teacher/students-list/{course_id}/{pageId}",
         new { controller = "TeacherHome", action = "StudentListInCourses" , pageId = UrlParameter.Optional },
         new[] { "OnlineQuizSystem.Areas.Teachers.Controllers" }
     );

            context.MapRoute(
           "Teacher_Quiz_Detail",
             "teacher/quiz-detail/{quiz_id}",
           new { controller = "TeacherHome", action = "QuizDetail", quiz_id = UrlParameter.Optional },
           new[] { "OnlineQuizSystem.Areas.Teachers.Controllers" }
        );

            context.MapRoute(
         "Teacher_Assignment_Detail",
           "teacher/assignment-detail/{assignment_id}",
         new { controller = "TeacherHome", action = "AssignmentDetail", assignment_id = UrlParameter.Optional },
         new[] { "OnlineQuizSystem.Areas.Teachers.Controllers" }
      );

            context.MapRoute(
        "Teacher_Courses_List",
          "teacher/courses/{pageId}",
        new { controller = "TeacherHome", action = "Courses", pageId = UrlParameter.Optional },
        new[] { "OnlineQuizSystem.Areas.Teachers.Controllers" }
    );


            context.MapRoute(
             "Teacher_Student_Quiz_Result_Detail",
             "teacher/student-result-detail/{quiz_id}/{student_id}",
             new { controller = "TeacherHome", action = "StudentResultDetail", quiz_id = UrlParameter.Optional, student_id = UrlParameter.Optional },
             new[] { "OnlineQuizSystem.Areas.Teachers.Controllers" }
         );

            context.MapRoute(
           "Teacher_Student_Assignment_Result_Detail",
           "teacher/assignment-result-detail/{assign_answers_id}/{student_id}",
           new { controller = "TeacherHome", action = "StudentAssignmentResultDetail", assign_answers_id = UrlParameter.Optional, student_id = UrlParameter.Optional },
           new[] { "OnlineQuizSystem.Areas.Teachers.Controllers" }
       );

            context.MapRoute(
              "Teacher_Home_Dashboard",
              "teacher/home",
              new { controller = "TeacherHome", action = "Index" },
              new[] { "OnlineQuizSystem.Areas.Teachers.Controllers" }
          );

            context.MapRoute(
                "Teachers_default",
                "Teachers/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }

    }
}