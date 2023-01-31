using DAL.Entities;
using DAL.Services;
using OnlineQuizSystem.CommonCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineQuizSystem.Areas.Teachers.Controllers
{

    [TeacherAuthorization]
    public class TeacherHomeController : Controller
    {
        // GET: Teachers/TeacherHome
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult QuizList(int? pageId, int? course_id, int? category_id, string title)
        {
            pageId = pageId == null || pageId == 0 ? 1 : pageId;
            int user_id = SessionManager.teacher.user_id;


            BasicDataModels model = new BasicDataModels();
            model.quiz_obj = new quiz();

            //--get quiz list
            model.quiz_list = TeacherServices.Instance.GetQuizList(pageId, Constants.ITEMS_PER_PAGE, course_id, category_id, title, user_id).ToList();

            //--get current page no
            model.quiz_obj.pageId = Convert.ToInt32(pageId);

            if (model.quiz_list.Count() > 0)
            {
                //--get total records count
                model.quiz_obj.TotalRecordCount = (int)model.quiz_list.FirstOrDefault().TotalRecordCount;
            }
            else
            {
                model.quiz_obj.TotalRecordCount = 0;
            }

            //--set total item per page
            model.quiz_obj.ItemsPerPage = Constants.ITEMS_PER_PAGE;

            if (Request.IsAjaxRequest())
            {
                return PartialView("~/Areas/Teachers/Views/TeacherHome/_QuizListPartial.cshtml", model);
            }



            //---get status list
            ViewBag.StatusList = AdminServices.Instance.GetStatusDropdownList().ToList();
            //--get course list for dropdwon GetCourseCategoriesDropDownList
            ViewBag.CoursesList = TeacherServices.Instance.GetCoursesListForTeacherDropDowns(user_id).ToList();
            //--get course list for dropdwon GetCourseCategoriesDropDownList
            ViewBag.QuizCategories = AdminServices.Instance.GetQuizCategoriesForDropDown().ToList();


            return View(model);
        }

        [HttpGet]
        public ActionResult QuizDetail(int? quiz_id)
        {
            quiz_id = quiz_id == null ? 2 : quiz_id;

            int created_by = SessionManager.teacher.user_id;


            BasicDataModels model = new BasicDataModels();
            model.quiz_categories_obj = new quiz_categories();

            //--get queiz detail
            model.quiz_obj = AdminServices.Instance.GetQuizDetailByID(quiz_id);
            //--get question list by quiz_id
            model.quiz_ques_list = TeacherServices.Instance.GetQuestionListByQuizId(quiz_id).ToList();


            //---get status list
            ViewBag.StatusList = AdminServices.Instance.GetStatusDropdownList().ToList();
            //--get course list for dropdwon GetCourseCategoriesDropDownList
            ViewBag.CoursesList = TeacherServices.Instance.GetCoursesListForTeacherDropDowns(created_by).ToList();
            //--get course list for dropdwon GetCourseCategoriesDropDownList
            ViewBag.QuizCategories = AdminServices.Instance.GetQuizCategoriesForDropDown().ToList();

            return View(model);
        }


        [HttpGet]
        public ActionResult StudentResultList(int? pageId, int? quiz_id)
        {
            pageId = pageId == null ? 1 : pageId;
            quiz_id = quiz_id == null ? 0 : quiz_id;


            BasicDataModels model = new BasicDataModels();
            model.student_result_obj = new student_result();

            model.student_result_list = TeacherServices.Instance.GetStudentsResultList(pageId, Constants.ITEMS_PER_PAGE, quiz_id).ToList();
            //--get current page no
            model.student_result_obj.pageId = Convert.ToInt32(pageId);

            if (model.student_result_list.Count() > 0)
            {
                //--get total records count
                model.student_result_obj.TotalRecordCount = (int)model.student_result_list.FirstOrDefault().TotalRecordCount;
            }
            else
            {
                model.student_result_obj.TotalRecordCount = 0;
            }
            //--set total item per page
            model.student_result_obj.ItemsPerPage = Constants.ITEMS_PER_PAGE;


            return PartialView(model);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditQuizDetail(quiz FormData)
        {
            //--getting calling page URL
            FormData.ReturnURL = this.Request.UrlReferrer.PathAndQuery;

            if (String.IsNullOrEmpty(FormData.title) || FormData.status == null || String.IsNullOrEmpty(FormData.start_date) || String.IsNullOrEmpty(FormData.end_date) || FormData.allowed_time_minutes < 1 || FormData.category_id == null || FormData.course_id < 1 || String.IsNullOrEmpty(FormData.description) || FormData.passing_marks < 1)
            {
                TempData["ErrorMsg"] = "Please fill all required fields!";

                return Redirect(FormData.ReturnURL);
            }

            //--Data update Part starts here
            FormData.modified_by = SessionManager.teacher.user_id;

            FormData.guid = CommonHelper.Instance.GeneratNewGUID();
            FormData.resultMsg = TeacherServices.Instance.UpdateQuiz(FormData);

            if (FormData.resultMsg == "Updated Successfully!")
            {
                TempData["SuccessMsg"] = "Updated Successfully!";
                return Redirect(FormData.ReturnURL);

            }
            else
            {
                TempData["ErrorMsg"] = FormData.resultMsg;
                return Redirect(FormData.ReturnURL);
            }

        }


        [HttpGet]
        public ActionResult AddQuiz()
        {
            int created_by = SessionManager.teacher.user_id;
            //---get status list
            ViewBag.StatusList = AdminServices.Instance.GetStatusDropdownList().ToList();

            //--get course list for dropdwon GetCourseCategoriesDropDownList
            ViewBag.CoursesList = TeacherServices.Instance.GetCoursesListForTeacherDropDowns(created_by).ToList();

            //--get course list for dropdwon GetCourseCategoriesDropDownList
            ViewBag.QuizCategories = AdminServices.Instance.GetQuizCategoriesForDropDown().ToList();

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddQuiz(quiz FormData)
        {

            //--getting calling page URL
            FormData.ReturnURL = this.Request.UrlReferrer.PathAndQuery;

            if (String.IsNullOrEmpty(FormData.title) || FormData.status == null || String.IsNullOrEmpty(FormData.start_date) || String.IsNullOrEmpty(FormData.end_date) || FormData.allowed_time_minutes < 1 || FormData.category_id == null || FormData.course_id < 1 || String.IsNullOrEmpty(FormData.description) || FormData.passing_marks < 1)
            {
                TempData["ErrorMsg"] = "Please fill all required fields!";

                return Redirect(FormData.ReturnURL);
            }

            //--Data Delete Part starts here
            FormData.created_by = SessionManager.teacher.user_id;
            FormData.guid = CommonHelper.Instance.GeneratNewGUID();
            FormData.resultMsg = TeacherServices.Instance.InsertNewQuiz(FormData);

            if (FormData.resultMsg == "Saved Successfully!")
            {
                FormData.quiz_id = AdminServices.Instance.GetQuizIdByGuid(FormData.guid);
                if (FormData.quiz_id < 1)
                {
                    TempData["ErrorMsg"] = "An error occured. Please try again";
                    return Redirect(FormData.ReturnURL);
                }
                else
                {
                    TempData["SuccessMsg"] = FormData.resultMsg;
                    return RedirectToAction("QuizDetail", "TeacherHome", new { quiz_id = FormData.quiz_id });
                }

            }
            else
            {
                TempData["ErrorMsg"] = FormData.resultMsg;
                return Redirect(FormData.ReturnURL);
            }

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteQuiz(quiz FormData)
        {

            //--getting calling page URL
            FormData.ReturnURL = this.Request.UrlReferrer.PathAndQuery;


            if (FormData.quiz_id < 1)
            {
                TempData["ErrorMsg"] = "Some thing went wrong. Please try again!";

                return Redirect(FormData.ReturnURL);
            }

            //--Data delete Part starts here
            FormData.resultMsg = TeacherServices.Instance.DeleteQuizDAL(FormData.quiz_id);

            if (FormData.resultMsg == "Deleted Successfully!")
            {

                TempData["SuccessMsg"] = "Deleted Successfully!";
                return Redirect(FormData.ReturnURL);


            }
            else
            {
                TempData["ErrorMsg"] = "Question not deleted. Please try again!";
                return Redirect(FormData.ReturnURL);
            }

        }

        [HttpGet]
        public ActionResult StudentResultDetail(int? quiz_id, int? student_id)
        {
            quiz_id = quiz_id == null ? 0 : quiz_id;
            student_id = student_id == null ? 0 : student_id;

            BasicDataModels model = new BasicDataModels();
            model.quiz_categories_obj = new quiz_categories();

            //--get queiz detail
            model.student_result_obj = TeacherServices.Instance.GetStudentsResultByQuizID_StudentID(quiz_id, student_id);
            //--get question list by quiz_id
            model.quiz_ques_list = TeacherServices.Instance.GetQuestionListByQuizId_StudentID(quiz_id, student_id).ToList();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddQuestion(quiz_questions FormData)
        {

            //--getting calling page URL
            FormData.ReturnURL = this.Request.UrlReferrer.PathAndQuery;

            if (String.IsNullOrEmpty(FormData.question_title) || String.IsNullOrEmpty(FormData.option_a) || String.IsNullOrEmpty(FormData.option_b) || String.IsNullOrEmpty(FormData.option_c) || String.IsNullOrEmpty(FormData.option_d))
            {
                TempData["ErrorMsg"] = "Please fill all required fields!";

                return Redirect(FormData.ReturnURL);
            }

            if (FormData.quiz_id < 1)
            {
                TempData["ErrorMsg"] = "Quiz data is null. Please try again!";

                return Redirect(FormData.ReturnURL);
            }

            //--Data insert Part starts here
            FormData.created_by = SessionManager.teacher.user_id;
            FormData.guid = CommonHelper.Instance.GeneratNewGUID();
            FormData.resultMsg = TeacherServices.Instance.InsertQuestion(FormData);

            if (FormData.resultMsg == "Saved Successfully!")
            {

                TempData["SuccessMsg"] = FormData.resultMsg;
                return Redirect(FormData.ReturnURL);


            }
            else
            {
                TempData["ErrorMsg"] = "Question not saved. Please try again!";
                return Redirect(FormData.ReturnURL);
            }

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditQuestion(quiz_questions FormData)
        {

            //--getting calling page URL
            FormData.ReturnURL = this.Request.UrlReferrer.PathAndQuery;

            if (String.IsNullOrEmpty(FormData.question_title) || String.IsNullOrEmpty(FormData.option_a) || String.IsNullOrEmpty(FormData.option_b) || String.IsNullOrEmpty(FormData.option_c) || String.IsNullOrEmpty(FormData.option_d))
            {
                TempData["ErrorMsg"] = "Please fill all required fields!";

                return Redirect(FormData.ReturnURL);
            }

            if (FormData.question_id < 1)
            {
                TempData["ErrorMsg"] = "Some thing went wrong. Please try again!";

                return Redirect(FormData.ReturnURL);
            }

            //--Data upate Part starts here
            FormData.modified_by = SessionManager.teacher.user_id;
            FormData.resultMsg = TeacherServices.Instance.UdpateQuestion(FormData);

            if (FormData.resultMsg == "Updated Successfully!")
            {

                TempData["SuccessMsg"] = FormData.resultMsg;
                return Redirect(FormData.ReturnURL);


            }
            else
            {
                TempData["ErrorMsg"] = "Question not updated. Please try again!";
                return Redirect(FormData.ReturnURL);
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteQuestion(quiz_questions FormData)
        {

            //--getting calling page URL
            FormData.ReturnURL = this.Request.UrlReferrer.PathAndQuery;


            if (FormData.question_id < 1)
            {
                TempData["ErrorMsg"] = "Some thing went wrong. Please try again!";

                return Redirect(FormData.ReturnURL);
            }

            //--Data delete Part starts here
            FormData.resultMsg = TeacherServices.Instance.DeleteQuestion(FormData.question_id);

            if (FormData.resultMsg == "Deleted Successfully!")
            {

                TempData["SuccessMsg"] = "Deleted Successfully!";
                return Redirect(FormData.ReturnURL);


            }
            else
            {
                TempData["ErrorMsg"] = "Question not deleted. Please try again!";
                return Redirect(FormData.ReturnURL);
            }

        }

        [HttpGet]
        public ActionResult Courses(int? pageId, string title, int? status, int? course_category_id)
        {
            pageId = pageId == null || pageId == 0 ? 1 : pageId;
            status = status == null || status == 0 ? 0 : status;
            course_category_id = course_category_id == null || course_category_id == 0 ? 0 : course_category_id;
            int user_id = SessionManager.teacher.user_id;


            BasicDataModels model = new BasicDataModels();
            model.courses_obj = new courses();


            model.courses_list = TeacherServices.Instance.GetCoursesList(pageId, Constants.ITEMS_PER_PAGE, title, status, course_category_id, user_id).ToList();

            model.courses_obj.pageId = Convert.ToInt32(pageId);
            if (model.courses_list.Count() > 0)
            {
                model.courses_obj.TotalRecordCount = (int)model.courses_list.FirstOrDefault().TotalRecordCount;
            }
            else
            {
                model.courses_obj.TotalRecordCount = 0;
            }

            model.courses_obj.ItemsPerPage = Constants.ITEMS_PER_PAGE;

            //---get status list
            ViewBag.StatusList = AdminServices.Instance.GetStatusDropdownList().ToList();

            //--get course categories for dropdwon GetCourseCategoriesDropDownList
            ViewBag.CourseCategories = AdminServices.Instance.GetCourseCategoriesDropDownList().ToList();

            if (Request.IsAjaxRequest())
            {
                return PartialView("~/Areas/Teachers/Views/TeacherHome/_CoursesPartial.cshtml", model);
            }


            return View(model);
        }

        [HttpGet]
        public ActionResult StudentsList(int? pageId, string user_name, string email, int? student_id)
        {
            pageId = pageId == null || pageId == 0 ? 1 : pageId;

            student_id = student_id == null || student_id == 0 ? 0 : student_id;

            BasicDataModels model = new BasicDataModels();
            model.student_obj= new students();


            model.student_list = TeacherServices.Instance.GetStudentsListsForTeachers(pageId, Constants.ITEMS_PER_PAGE, user_name, email, student_id).ToList();

            model.student_obj.pageId = Convert.ToInt32(pageId);
            if (model.student_list.Count() > 0)
            {
                model.student_obj.TotalRecordCount = (int)model.student_list.FirstOrDefault().TotalRecordCount;
            }
            else
            {
                model.student_obj.TotalRecordCount = 0;
            }

            model.student_obj.ItemsPerPage = Constants.ITEMS_PER_PAGE;

            //---get status list
            ViewBag.StatusList = AdminServices.Instance.GetStatusDropdownList().ToList();

            //--get course categories for dropdwon GetCourseCategoriesDropDownList
            ViewBag.CourseCategories = AdminServices.Instance.GetCourseCategoriesDropDownList().ToList();

            if (Request.IsAjaxRequest())
            {
                return PartialView("~/Areas/Teachers/Views/TeacherHome/_StudentsListPartial.cshtml", model);
            }


            return View(model);
        }

        [HttpGet]
        public ActionResult AssignmentList(int? pageId, int? course_id, string title)
        {
            pageId = pageId == null || pageId == 0 ? 1 : pageId;
            int user_id = SessionManager.teacher.user_id;


            BasicDataModels model = new BasicDataModels();
            model.assignment_obj = new assignments();

            //--get quiz list
            model.assignment_list = TeacherServices.Instance.GetAssignmentListForTeachers(pageId, Constants.ITEMS_PER_PAGE, course_id, title, user_id).ToList();

            //--get current page no
            model.assignment_obj.pageId = Convert.ToInt32(pageId);

            if (model.assignment_list.Count() > 0)
            {
                //--get total records count
                model.assignment_obj.TotalRecordCount = (int)model.assignment_list.FirstOrDefault().TotalRecordCount;
            }
            else
            {
                model.assignment_obj.TotalRecordCount = 0;
            }

            //--set total item per page
            model.assignment_obj.ItemsPerPage = Constants.ITEMS_PER_PAGE;

            if (Request.IsAjaxRequest())
            {
                return PartialView("~/Areas/Teachers/Views/TeacherHome/_AssignmentListPartial.cshtml", model);
            }



            //---get status list
            ViewBag.StatusList = AdminServices.Instance.GetStatusDropdownList().ToList();
            //--get course list for dropdwon GetCourseCategoriesDropDownList
            ViewBag.CoursesList = TeacherServices.Instance.GetCoursesListForTeacherDropDowns(user_id).ToList();
          

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteAssignment(assignments FormData)
        {

            //--getting calling page URL
            FormData.ReturnURL = this.Request.UrlReferrer.PathAndQuery;


            if (FormData.assignment_id < 1)
            {
                TempData["ErrorMsg"] = "Some thing went wrong. Please try again!";

                return Redirect(FormData.ReturnURL);
            }

            //--Data delete Part starts here
            FormData.resultMsg = TeacherServices.Instance.DeleteAssignment(FormData.assignment_id);

            if (FormData.resultMsg == "Deleted Successfully!")
            {

                TempData["SuccessMsg"] = "Deleted Successfully!";
                return Redirect(FormData.ReturnURL);


            }
            else
            {
                TempData["ErrorMsg"] = "Question not deleted. Please try again!";
                return Redirect(FormData.ReturnURL);
            }

        }


        [HttpGet]
        public ActionResult AddAssignment()
        {
            int created_by = SessionManager.teacher.user_id;
            //---get status list
            ViewBag.StatusList = AdminServices.Instance.GetStatusDropdownList().ToList();

            //--get course list for dropdwon GetCourseCategoriesDropDownList
            ViewBag.CoursesList = TeacherServices.Instance.GetCoursesListForTeacherDropDowns(created_by).ToList();

         

            return View();
        }

       
        //I have added here ValidateInput(false) bcz i am sending an html string in assignment body
        [HttpPost, ValidateInput(false)]
        public ActionResult AddAssignment(assignments FormData)
        {

            //--getting calling page URL
            FormData.ReturnURL = this.Request.UrlReferrer.PathAndQuery;

            if (String.IsNullOrEmpty(FormData.title) || FormData.status == null || String.IsNullOrEmpty(FormData.start_date) || String.IsNullOrEmpty(FormData.end_date) || FormData.allowed_time_minutes < 1  || FormData.course_id < 1  || FormData.passing_marks < 1 || FormData.total_marks < 1 || String.IsNullOrEmpty(FormData.body))
            {
                TempData["ErrorMsg"] = "Please fill all required fields!";

                return Redirect(FormData.ReturnURL);
            }

            //--Data Delete Part starts here
            FormData.created_by = SessionManager.teacher.user_id;
            FormData.guid = CommonHelper.Instance.GeneratNewGUID();
            FormData.resultMsg = TeacherServices.Instance.InsertNewAssignment(FormData);

            if (FormData.resultMsg == "Saved Successfully!")
            {
                FormData.assignment_id = TeacherServices.Instance.GetAssignmentIdByGuid(FormData.guid);
                if (FormData.assignment_id < 1)
                {
                    TempData["ErrorMsg"] = "An error occured. Please try again";
                    return Redirect(FormData.ReturnURL);
                }
                else
                {
                    TempData["SuccessMsg"] = FormData.resultMsg;
                    return RedirectToAction("AssignmentDetail", "TeacherHome", new { assignment_id = FormData.assignment_id });
                }

            }
            else
            {
                TempData["ErrorMsg"] = FormData.resultMsg;
                return Redirect(FormData.ReturnURL);
            }

        }


        [HttpGet]
        public ActionResult AssignmentDetail(int? assignment_id)
        {
            assignment_id = assignment_id == null ? 2 : assignment_id;

            int user_id = SessionManager.teacher.user_id;


            BasicDataModels model = new BasicDataModels();
            model.assignment_obj = new assignments();

            //--get queiz detail
            model.assignment_obj = TeacherServices.Instance.GetAssignmentDetailByID(assignment_id);
           
            //---get status list
            ViewBag.StatusList = AdminServices.Instance.GetStatusDropdownList().ToList();
            //--get course list for dropdwon GetCourseCategoriesDropDownList
            ViewBag.CoursesList = TeacherServices.Instance.GetCoursesListForTeacherDropDowns(user_id).ToList();
          
            return View(model);
        }

        //I have added here ValidateInput(false) bcz i am sending an html string in assignment body
        [HttpPost, ValidateInput(false)]
        public ActionResult EditAssignmentDetail(assignments FormData)
        {
            //--getting calling page URL
            FormData.ReturnURL = this.Request.UrlReferrer.PathAndQuery;

            if (String.IsNullOrEmpty(FormData.title) || FormData.status == null || String.IsNullOrEmpty(FormData.start_date) || String.IsNullOrEmpty(FormData.end_date) || FormData.allowed_time_minutes < 1 || String.IsNullOrEmpty(FormData.body) || FormData.course_id < 1  || FormData.passing_marks < 1)
            {
                TempData["ErrorMsg"] = "Please fill all required fields!";

                return Redirect(FormData.ReturnURL);
            }

            //--Data update Part starts here
            FormData.modified_by = SessionManager.teacher.user_id;

            FormData.guid = CommonHelper.Instance.GeneratNewGUID();
            FormData.resultMsg = TeacherServices.Instance.UpdateAssignment(FormData);

            if (FormData.resultMsg == "Updated Successfully!")
            {
                TempData["SuccessMsg"] = "Updated Successfully!";
                return Redirect(FormData.ReturnURL);

            }
            else
            {
                TempData["ErrorMsg"] = FormData.resultMsg;
                return Redirect(FormData.ReturnURL);
            }

        }


        [HttpGet]
        public ActionResult StudentAssignmentResultList(int? pageId, int? assignment_id)
        {
            pageId = pageId == null ? 1 : pageId;
            assignment_id = assignment_id == null ? 0 : assignment_id;


            BasicDataModels model = new BasicDataModels();
            model.std_assignment_result_obj = new student_assignment_result();

            model.std_assignment_result_list = TeacherServices.Instance.GetStudentsAssignmentResultList(pageId, Constants.ITEMS_PER_PAGE, assignment_id).ToList();
            //--get current page no
            model.std_assignment_result_obj.pageId = Convert.ToInt32(pageId);

            if (model.std_assignment_result_list.Count() > 0)
            {
                //--get total records count
                model.std_assignment_result_obj.TotalRecordCount = (int)model.std_assignment_result_list.FirstOrDefault().TotalRecordCount;
            }
            else
            {
                model.std_assignment_result_obj.TotalRecordCount = 0;
            }
            //--set total item per page
            model.std_assignment_result_obj.ItemsPerPage = Constants.ITEMS_PER_PAGE;


            return PartialView(model);
        }

        [HttpGet]
        public ActionResult StudentAssignmentResultDetail(int? assign_answers_id, int? student_id)
        {
            assign_answers_id = assign_answers_id == null ? -1 : assign_answers_id;
            student_id = student_id == null ? -1 : student_id;


            BasicDataModels model = new BasicDataModels();
            model.assignment_obj = new assignments();


            //--get queiz detail
            model.assignment_obj = TeacherServices.Instance.GetAssignmentDetailForStudentForTeacher(assign_answers_id, student_id);


            return View(model);
        }

        [HttpPost]
        public ActionResult GiveAssignmentMarkToStudent(assignment_answers FormData)
        {
            BasicDataModels model = new BasicDataModels();

            //--getting calling page URL
            FormData.ReturnURL = this.Request.UrlReferrer.PathAndQuery;

            if (FormData.assign_answers_id<1 || FormData.gain_marks<1)
            {
                TempData["ErrorMsg"] = "Please fill all required fields!";
                FormData.ErrorMsg = "Please fill all required fields!";
                return Redirect(FormData.ReturnURL);
            }

            //--Data Insert Part starts here
            FormData.created_by = SessionManager.teacher.user_id;
            FormData.ErrorMsg = TeacherServices.Instance.GiveAssignmentMarksToStudent(FormData);
            if (FormData.ErrorMsg == "Saved Successfully!")
            {
                TempData["SuccessMsg"] = FormData.ErrorMsg;
                return Redirect(FormData.ReturnURL);
            }
            else
            {
                TempData["ErrorMsg"] = FormData.ErrorMsg;
                return Redirect(FormData.ReturnURL);
            }

          
        }

        [HttpGet]
        public ActionResult StudentListInCourses(int? pageId,int? course_id, string user_name, int? student_id)
        {
            pageId = pageId == null || pageId == 0 ? 1 : pageId;
            student_id = student_id == null || student_id == 0 ? 0 : student_id;
            course_id = course_id == null || course_id == 0 ? 0 : course_id;
            ViewBag.course_id = course_id;

            int user_id = SessionManager.teacher.user_id;


            BasicDataModels model = new BasicDataModels();
            model.student_courses_obj = new student_courses();


            model.student_courses_list = TeacherServices.Instance.GetStudentsListForCourse(pageId, Constants.ITEMS_PER_PAGE, course_id, user_name, student_id).ToList();

            model.student_courses_obj.pageId = Convert.ToInt32(pageId);
            if (model.student_courses_list.Count() > 0)
            {
                model.student_courses_obj.TotalRecordCount = (int)model.student_courses_list.FirstOrDefault().TotalRecordCount;
            }
            else
            {
                model.student_courses_obj.TotalRecordCount = 0;
            }

            model.student_courses_obj.ItemsPerPage = Constants.ITEMS_PER_PAGE;

         

            if (Request.IsAjaxRequest())
            {
                return PartialView("~/Areas/Teachers/Views/TeacherHome/_StudentListInCoursesPartial.cshtml", model);
            }


            return View(model);
        }

    }
}