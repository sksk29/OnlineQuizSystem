using DAL.Entities;
using DAL.Services;
using OnlineQuizSystem.CommonCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineQuizSystem.Areas.Users.Controllers
{
    [StudentAuthorization]
    public class UserHomeController : Controller
    {
        // GET: Users/UserHome
        [HttpGet]
       
        public ActionResult Index()
        {

            int? student_id = SessionManager.student == null ? 0 : SessionManager.student.student_id;
            BasicDataModels model = new BasicDataModels();

            //--get course list by student id
            model.courses_list = StudentServices.Instance.GetCoursesForDropDownByStudentID(student_id);

            return View(model);
        }

        [HttpGet]
        public ActionResult CoursesList(int? pageId, string title, int? status, int? course_category_id)
        {
            pageId = pageId == null || pageId == 0 ? 1 : pageId;
            status = status == null || status == 0 ? 0 : status;
            course_category_id = course_category_id == null || course_category_id == 0 ? 0 : course_category_id;

            int? student_id = SessionManager.student == null ? 0 : SessionManager.student.student_id;

            BasicDataModels model = new BasicDataModels();
            model.courses_obj = new courses();


            model.courses_list = StudentServices.Instance.GetStudentMainCoursesListByStudentID(pageId, Constants.ITEMS_PER_PAGE, title, status, course_category_id, student_id).ToList();

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
                return PartialView("~/Areas/Users/Views/UserHome/_CoursesListPartial.cshtml", model);
            }


            return View(model);
        }

        [HttpGet]
        //[StudentAuthorization]
        public ActionResult StudentQuizList(int? course_id, int? pageId, string title)
        {
            course_id = course_id == null ? 0 : course_id;

            ViewBag.course_id = course_id;

            int? student_id = SessionManager.student == null ? 0 : SessionManager.student.student_id;

            BasicDataModels model = new BasicDataModels();
            model.quiz_obj = new quiz();
            //--get quiz list by student id
            model.quiz_list = StudentServices.Instance.GetQuizListForStudentExamByCourseID(pageId, Constants.ITEMS_PER_PAGE, student_id, course_id, title);
            //--assign page id to model
            model.quiz_obj.pageId = Convert.ToInt32(pageId);

            if (model.quiz_list.Count() > 0)//---assign total count to model
            {
                model.quiz_obj.TotalRecordCount = (int)model.quiz_list.FirstOrDefault().TotalRecordCount;
            }
            else
            {
                model.quiz_obj.TotalRecordCount = 0;
            }
            //--assign items per page
            model.quiz_obj.ItemsPerPage = Constants.ITEMS_PER_PAGE;

            //--if it is ajax request
            if (Request.IsAjaxRequest())
            {
                return PartialView("~/Areas/Users/Views/UserHome/_StudentQuizListPartial.cshtml", model);
            }

            return View(model);
        }

        [HttpGet]
       
        public ActionResult StartQuiz(int? quiz_ID)
        {

            quiz_ID = quiz_ID == null ? 2 : quiz_ID;

            BasicDataModels model = new BasicDataModels();
            model.quiz_categories_obj = new quiz_categories();

            //--get queiz detail
            model.quiz_obj = AdminServices.Instance.GetQuizDetailByID(quiz_ID);
            //--get question list by quiz_id
            model.quiz_ques_list = StudentServices.Instance.GetQuestionListByQuizId(quiz_ID).ToList();

            return View(model);
        }

        [HttpPost]
      
        public ActionResult SaveAnswer(int question_id, string correct_option)
        {



            if (String.IsNullOrEmpty(correct_option))
            {
               
                return Json("an error occured. please try again", JsonRequestBehavior.AllowGet);
            }

            //--Data upate Part starts here
            int student_id = SessionManager.student.student_id;
            string guid = CommonHelper.Instance.GeneratNewGUID();
            string resultMsg = StudentServices.Instance.SaveQuizAnswer(student_id, question_id, correct_option, guid);

            if (resultMsg == "Updated Successfully!")
            {
                return Json("saved successfully", JsonRequestBehavior.AllowGet);
                //return Content("saved successfully");

            }
            else
            {
                return Json("an error occured. please try again", JsonRequestBehavior.AllowGet);
             
            }


        }


        [HttpPost]
      
        public ActionResult updateQuizRemainingTime(int quiz_ID, int remaining_minutes)
        {


            //--Data upate Part starts here
            int student_id = SessionManager.student.student_id;

            string resultMsg = StudentServices.Instance.updateQuizRemainingTime(quiz_ID, student_id, remaining_minutes);

            if (resultMsg == "Updated Successfully!")
            {

                //return Content("saved successfully");
                return Json("saved successfully", JsonRequestBehavior.AllowGet);

            }
            else
            {
           
                return Json("an error occured. please try again", JsonRequestBehavior.AllowGet);
            }


        }

        [HttpGet]
        public JsonResult GetStudentsResult(int quiz_id)

        {
            int student_id = SessionManager.student.student_id;

            BasicDataModels model = new BasicDataModels();
            model.student_result_obj = new student_result();
            //---get status list
            model.student_result_obj = StudentServices.Instance.GetStudentsResultDetail(student_id, quiz_id);
         
            if (model.student_result_obj != null)
            {
               
                    return Json(model.student_result_obj, JsonRequestBehavior.AllowGet);
             

            }
            else
            {
                return Json(new { message = "No data found" }, JsonRequestBehavior.AllowGet);
            }


        }

        [HttpGet]
        //[StudentAuthorization]
        public ActionResult StudentAssignmentList(int? course_id, int? pageId, string title)
        {
            course_id = course_id == null ? 0 : course_id;

            ViewBag.course_id = course_id;

            int? student_id = SessionManager.student == null ? 0 : SessionManager.student.student_id;

            BasicDataModels model = new BasicDataModels();
            model.assignment_obj = new assignments();
            //--get quiz list by student id
            model.assignment_list = StudentServices.Instance.GetAssignmentListForStudentByCourseID(pageId, Constants.ITEMS_PER_PAGE, student_id, course_id, title);
            //--assign page id to model
            model.assignment_obj.pageId = Convert.ToInt32(pageId);

            if (model.assignment_list.Count() > 0)//---assign total count to model
            {
                model.assignment_obj.TotalRecordCount = (int)model.assignment_list.FirstOrDefault().TotalRecordCount;
            }
            else
            {
                model.assignment_obj.TotalRecordCount = 0;
            }
            //--assign items per page
            model.assignment_obj.ItemsPerPage = Constants.ITEMS_PER_PAGE;

            //--if it is ajax request
            if (Request.IsAjaxRequest())
            {
                return PartialView("~/Areas/Users/Views/UserHome/_StudentAssignmentListPartial.cshtml", model);
            }

            return View(model);
        }

        [HttpGet]
        public JsonResult GetStudentsAssignmentResultByAssignmentID(int assignment_id)

        {
            int student_id = SessionManager.student.student_id;

            BasicDataModels model = new BasicDataModels();
            model.std_assignment_result_obj = new student_assignment_result();
            //---get status list
            model.std_assignment_result_obj = StudentServices.Instance.GetStudentAssignmentResultByAssignmentID(student_id, assignment_id);

            if (model.std_assignment_result_obj != null)
            {

                return Json(model.std_assignment_result_obj, JsonRequestBehavior.AllowGet);


            }
            else
            {
                return Json(new { message = "No data found" }, JsonRequestBehavior.AllowGet);
            }


        }


        [HttpGet]

        public ActionResult StartAssignment(int? assignment_id)
        {

            assignment_id = assignment_id == null ? -1 : assignment_id;
            int student_id = SessionManager.student.student_id;

            BasicDataModels model = new BasicDataModels();
            model.assignment_obj = new assignments();

         
            //--get queiz detail
            model.assignment_obj = StudentServices.Instance.GetAssignmentDetailForStudentByID(assignment_id, student_id);
         

            return View(model);
        }

        [HttpPost]

        public ActionResult updateAssignmentRemainingTime(int assignment_id, int remaining_minutes)
        {


            //--Data upate Part starts here
            int student_id = SessionManager.student.student_id;

            string resultMsg = StudentServices.Instance.updateAssignmentRemainingTime(assignment_id, student_id, remaining_minutes);

            if (resultMsg == "Updated Successfully!")
            {

              
                return Json("saved successfully", JsonRequestBehavior.AllowGet);
            }
            else
            {
            
                return Json("an error occured. please try again", JsonRequestBehavior.AllowGet);
            }


        }

        //I have added here ValidateInput(false) bcz i am sending an html string in assignment body
        [HttpPost, ValidateInput(false)]
        public ActionResult SaveStudentAssignmentAnswer(assignment_answers FormData)
        {

            FormData.ReturnURL = this.Request.UrlReferrer.PathAndQuery;

            if (String.IsNullOrEmpty(FormData.student_ans)  || FormData.assignment_id<1)
            {
                TempData["ErrorMsg"] = "Please fill all required fields!";
                //--getting calling page URL
                return Redirect(FormData.ReturnURL);
            }

            //--Data Delete Part starts here
            FormData.created_by = SessionManager.student.student_id;
            FormData.guid = CommonHelper.Instance.GeneratNewGUID();
            FormData.resultMsg = StudentServices.Instance.SaveStudentAssignmentAnswer(FormData);

            if (FormData.resultMsg == "Saved Successfully!")
            {

                //--here return url coming from the startAssignment page
                TempData["SuccessMsg"] = FormData.resultMsg;
                return RedirectToAction("CoursesList", "UserHome" , new { pageId =1});
              

            }
            else
            {
             
                TempData["ErrorMsg"] = FormData.resultMsg;
                return Redirect(FormData.ReturnURL);
            }

        }

    }
}