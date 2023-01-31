using DAL.Entities;
using DAL.Services;
using OnlineQuizSystem.CommonCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineQuizSystem.Areas.Admin.Controllers
{
    public class AdminHomeController : AdminBaseController
    {
        // GET: Admin/AdminHome
        public ActionResult Dashboard()
        {
            BasicDataModels model = new BasicDataModels();
            model.ChartDataList = AdminServices.Instance.GetDashboardChartData(); //--Leads chart data
            return View(model);
        }

        [HttpGet]
        public ActionResult Courses(int? pageId, string title, int? status, int? course_category_id)
        {
            pageId = pageId == null || pageId == 0 ? 1 : pageId;
            status = status == null || status == 0 ? 0 : status;
            course_category_id = course_category_id == null || course_category_id == 0 ? 0 : course_category_id;

            BasicDataModels model = new BasicDataModels();
            model.courses_obj = new courses();


            model.courses_list = AdminServices.Instance.GetCoursesList(pageId, Constants.ITEMS_PER_PAGE, title, status, course_category_id).ToList();

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
                return PartialView("~/Areas/Admin/Views/AdminHome/_CoursesPartial.cshtml", model);
            }


            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddNewCourse(courses FormData)
        {
            BasicDataModels model = new BasicDataModels();

            //--getting calling page URL
            FormData.ReturnURL = this.Request.UrlReferrer.PathAndQuery;

            if (String.IsNullOrEmpty(FormData.title) || FormData.status == null || FormData.course_category_id == null || String.IsNullOrEmpty(FormData.description))
            {
                TempData["ErrorMsg"] = "Please fill all required fields!";
                return Redirect(FormData.ReturnURL);
            }

            //--Data Insert Part starts here
            FormData.created_by = SessionManager.user.user_id;
            FormData.guid = CommonHelper.Instance.GeneratNewGUID();
            FormData.resultMsg = AdminServices.Instance.InsertNewCourse(FormData);
            if (FormData.resultMsg == "Saved Successfully!")
            {
                TempData["SuccessMsg"] = FormData.resultMsg;
                return Redirect(FormData.ReturnURL);
            }
            else
            {
                TempData["ErrorMsg"] = FormData.resultMsg;
                return Redirect(FormData.ReturnURL);
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditCourse(courses FormData)
        {
            BasicDataModels model = new BasicDataModels();

            //--getting calling page URL
            FormData.ReturnURL = this.Request.UrlReferrer.PathAndQuery;


            if (FormData.course_id < 1)
            {
                TempData["ErrorMsg"] = "Course ID is null!";
                return Redirect(FormData.ReturnURL);
            }

            if (String.IsNullOrEmpty(FormData.title) || FormData.status == null || FormData.course_category_id == null || String.IsNullOrEmpty(FormData.description))
            {
                TempData["ErrorMsg"] = "Please fill all required fields!";
                return Redirect(FormData.ReturnURL);
            }

            //--Data update Part starts here
            FormData.modified_by = SessionManager.user.user_id;
            FormData.resultMsg = AdminServices.Instance.UpdateCourse(FormData);
            if (FormData.resultMsg == "Updated Successfully!")
            {
                TempData["SuccessMsg"] = FormData.resultMsg;
                return Redirect(FormData.ReturnURL);
            }
            else
            {
                TempData["ErrorMsg"] = FormData.resultMsg;
                return Redirect(FormData.ReturnURL);
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteCourse(int? course_id)
        {
            courses FormData = new courses();


            //--getting calling page URL
            FormData.ReturnURL = this.Request.UrlReferrer.PathAndQuery;

            if (course_id == null)
            {
                TempData["ErrorMsg"] = "Some thing went wrong. Please try again!";

                return Redirect(FormData.ReturnURL);
            }

            //--Data Delete Part starts here
            FormData.resultMsg = AdminServices.Instance.DeleteCourse(course_id);

            if (FormData.resultMsg == "Deleted Successfully!")
            {
                TempData["SuccessMsg"] = FormData.resultMsg;
                return Redirect(FormData.ReturnURL);
            }
            else
            {
                TempData["ErrorMsg"] = FormData.resultMsg;
                return Redirect(FormData.ReturnURL);
            }

        }


        [HttpGet]
        public ActionResult StudentsList(int? pageId, string user_name, string email, int? student_id)
        {
            pageId = pageId == null || pageId == 0 ? 1 : pageId;

            student_id = student_id == null || student_id == 0 ? 0 : student_id;

            BasicDataModels model = new BasicDataModels();
            model.student_obj = new students();


            model.student_list = AdminServices.Instance.GetStudentsListsForAdmin(pageId, Constants.ITEMS_PER_PAGE, user_name, email, student_id).ToList();

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


            if (Request.IsAjaxRequest())
            {
                return PartialView("~/Areas/Admin/Views/AdminHome/_StudentsListPartial.cshtml", model);
            }


            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddStudent(students FormData)
        {
            BasicDataModels model = new BasicDataModels();

            //--getting calling page URL
            FormData.ReturnURL = this.Request.UrlReferrer.PathAndQuery;

            if (String.IsNullOrEmpty(FormData.first_name) || String.IsNullOrEmpty(FormData.last_name) || String.IsNullOrEmpty(FormData.user_name) || String.IsNullOrEmpty(FormData.email))
            {
                TempData["ErrorMsg"] = "Please fill all required fields!";
                return Redirect(FormData.ReturnURL);
            }

            //--Data Insert Part starts here
            FormData.created_by = SessionManager.user.user_id;
            FormData.guid = CommonHelper.Instance.GeneratNewGUID();
            FormData.password = CommonHelper.Instance.GeneratRandomNumber();
            FormData.resultMsg = AdminServices.Instance.InsertNewStudent(FormData);
            if (FormData.resultMsg == "Saved Successfully!")
            {
                TempData["SuccessMsg"] = FormData.resultMsg;
                return Redirect(FormData.ReturnURL);
            }
            else
            {
                TempData["ErrorMsg"] = FormData.resultMsg;
                return Redirect(FormData.ReturnURL);
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditStudent(students FormData)
        {
            BasicDataModels model = new BasicDataModels();

            //--getting calling page URL
            FormData.ReturnURL = this.Request.UrlReferrer.PathAndQuery;


            if (FormData.student_id < 1)
            {
                TempData["ErrorMsg"] = "Student ID is null!";
                return Redirect(FormData.ReturnURL);
            }

            if (String.IsNullOrEmpty(FormData.first_name) || String.IsNullOrEmpty(FormData.last_name) || String.IsNullOrEmpty(FormData.user_name) || String.IsNullOrEmpty(FormData.email))
            {
                TempData["ErrorMsg"] = "Please fill all required fields!";
                return Redirect(FormData.ReturnURL);
            }

            //--Data update Part starts here
            FormData.modified_by = SessionManager.user.user_id;
            FormData.resultMsg = AdminServices.Instance.EditStudent(FormData);
            if (FormData.resultMsg == "Updated Successfully!")
            {
                TempData["SuccessMsg"] = FormData.resultMsg;
                return Redirect(FormData.ReturnURL);
            }
            else
            {
                TempData["ErrorMsg"] = FormData.resultMsg;
                return Redirect(FormData.ReturnURL);
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteStudent(int? student_id)
        {
            courses FormData = new courses();


            //--getting calling page URL
            FormData.ReturnURL = this.Request.UrlReferrer.PathAndQuery;

            if (student_id == null)
            {
                TempData["ErrorMsg"] = "Some thing went wrong. Please try again!";

                return Redirect(FormData.ReturnURL);
            }

            //--Data Delete Part starts here
            FormData.resultMsg = AdminServices.Instance.DeleteStudent(student_id);

            if (FormData.resultMsg == "Deleted Successfully!")
            {
                TempData["SuccessMsg"] = FormData.resultMsg;
                return Redirect(FormData.ReturnURL);
            }
            else
            {
                TempData["ErrorMsg"] = FormData.resultMsg;
                return Redirect(FormData.ReturnURL);
            }

        }

        [HttpGet]
        public ActionResult StudentRegisteredCourses(int? pageId, int student_id, string title, int? course_id)
        {
            pageId = pageId == null || pageId == 0 ? 1 : pageId;
            course_id = course_id == null || course_id == 0 ? 0 : course_id;

            ViewBag.student_id = student_id;
            BasicDataModels model = new BasicDataModels();
            model.courses_obj = new courses();


            model.courses_list = AdminServices.Instance.GetStudentRegisteredCourses(pageId, Constants.ITEMS_PER_PAGE, student_id, title, course_id).ToList();

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


            if (Request.IsAjaxRequest())
            {
                return PartialView("~/Areas/Admin/Views/AdminHome/_StudentRegisteredCoursesPartial.cshtml", model);
            }


            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AssignNewCourseToStudent(student_courses FormData)
        {
            BasicDataModels model = new BasicDataModels();

            //--getting calling page URL
            FormData.ReturnURL = this.Request.UrlReferrer.PathAndQuery;


            if (FormData.student_id < 1)
            {
                TempData["ErrorMsg"] = "Student ID is null!";
                return Redirect(FormData.ReturnURL);
            }

            if (FormData.course_id < 1)
            {
                TempData["ErrorMsg"] = "Please fill all required fields!";
                return Redirect(FormData.ReturnURL);
            }

            //--Data update Part starts here
            FormData.created_by = SessionManager.user.user_id;
            FormData.resultMsg = AdminServices.Instance.AssignCourseToStudent(FormData);
            if (FormData.resultMsg == "Saved Successfully!")
            {
                TempData["SuccessMsg"] = FormData.resultMsg;
                return Redirect(FormData.ReturnURL);
            }
            else
            {
                TempData["ErrorMsg"] = FormData.resultMsg;
                return Redirect(FormData.ReturnURL);
            }

        }

        [HttpGet]
        public JsonResult GetStudentsUnassignCourseList(int student_id, string course_title)

        {
            BasicDataModels model = new BasicDataModels();
            //---get status list
            model.courses_list = AdminServices.Instance.GetStudentsUnAssignCourseList(student_id, course_title).ToList();

            if (model.courses_list != null)
            {
                if (model.courses_list.Count() > 0)
                {
                    return Json(model.courses_list, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { message = "No data found" }, JsonRequestBehavior.AllowGet);
                }

            }
            else
            {
                return Json(new { message = "No data found" }, JsonRequestBehavior.AllowGet);
            }


        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RemoveStudentFromCourse(int student_course_id)
        {
            courses FormData = new courses();


            //--getting calling page URL
            FormData.ReturnURL = this.Request.UrlReferrer.PathAndQuery;

            if (student_course_id < 1)
            {
                TempData["ErrorMsg"] = "Some thing went wrong. Please try again!";

                return Redirect(FormData.ReturnURL);
            }

            //--Data Delete Part starts here
            FormData.resultMsg = AdminServices.Instance.RemoveCourseFromStudentList(student_course_id);

            if (FormData.resultMsg == "Deleted Successfully!")
            {
                TempData["SuccessMsg"] = FormData.resultMsg;
                return Redirect(FormData.ReturnURL);
            }
            else
            {
                TempData["ErrorMsg"] = FormData.resultMsg;
                return Redirect(FormData.ReturnURL);
            }

        }

        //-------------------------------
        [HttpGet]
        public ActionResult TeachersList(int? pageId, string user_name, string email, int? user_id)
        {
            pageId = pageId == null || pageId == 0 ? 1 : pageId;

            user_id = user_id == null || user_id == 0 ? 0 : user_id;
            int user_type = 2; //2 is teacher in user table

            BasicDataModels model = new BasicDataModels();
            model.user_obj = new users();


            model.user_list = AdminServices.Instance.GetTeachersListsForAdmin(pageId, Constants.ITEMS_PER_PAGE, user_name, email, user_id, user_type).ToList();

            model.user_obj.pageId = Convert.ToInt32(pageId);
            if (model.user_list.Count() > 0)
            {
                model.user_obj.TotalRecordCount = (int)model.user_list.FirstOrDefault().TotalRecordCount;
            }
            else
            {
                model.user_obj.TotalRecordCount = 0;
            }

            model.user_obj.ItemsPerPage = Constants.ITEMS_PER_PAGE;


            if (Request.IsAjaxRequest())
            {
                return PartialView("~/Areas/Admin/Views/AdminHome/_TeachersListPartial.cshtml", model);
            }


            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddTeacher(users FormData)
        {
            BasicDataModels model = new BasicDataModels();

            //--getting calling page URL
            FormData.ReturnURL = this.Request.UrlReferrer.PathAndQuery;

            if (String.IsNullOrEmpty(FormData.first_name) || String.IsNullOrEmpty(FormData.last_name) || String.IsNullOrEmpty(FormData.user_name) || String.IsNullOrEmpty(FormData.email))
            {
                TempData["ErrorMsg"] = "Please fill all required fields!";
                return Redirect(FormData.ReturnURL);
            }

            //--Data Insert Part starts here
            FormData.created_by = SessionManager.user.user_id;
            FormData.user_type = 2; //2 is teacher type
            FormData.password = CommonHelper.Instance.GeneratRandomNumber();
            FormData.resultMsg = AdminServices.Instance.InsertNewTeacher(FormData);
            if (FormData.resultMsg == "Saved Successfully!")
            {
                TempData["SuccessMsg"] = FormData.resultMsg;
                return Redirect(FormData.ReturnURL);
            }
            else
            {
                TempData["ErrorMsg"] = FormData.resultMsg;
                return Redirect(FormData.ReturnURL);
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditTeacher(users FormData)
        {
            BasicDataModels model = new BasicDataModels();

            //--getting calling page URL
            FormData.ReturnURL = this.Request.UrlReferrer.PathAndQuery;


            if (FormData.user_id < 1)
            {
                TempData["ErrorMsg"] = "Student ID is null!";
                return Redirect(FormData.ReturnURL);
            }

            if (String.IsNullOrEmpty(FormData.first_name) || String.IsNullOrEmpty(FormData.last_name) || String.IsNullOrEmpty(FormData.user_name) || String.IsNullOrEmpty(FormData.email))
            {
                TempData["ErrorMsg"] = "Please fill all required fields!";
                return Redirect(FormData.ReturnURL);
            }

            //--Data update Part starts here
            FormData.modified_by = SessionManager.user.user_id;
            FormData.resultMsg = AdminServices.Instance.EditTeacher(FormData);
            if (FormData.resultMsg == "Updated Successfully!")
            {
                TempData["SuccessMsg"] = FormData.resultMsg;
                return Redirect(FormData.ReturnURL);
            }
            else
            {
                TempData["ErrorMsg"] = FormData.resultMsg;
                return Redirect(FormData.ReturnURL);
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteTeacher(int? user_id)
        {
            courses FormData = new courses();


            //--getting calling page URL
            FormData.ReturnURL = this.Request.UrlReferrer.PathAndQuery;

            if (user_id == null)
            {
                TempData["ErrorMsg"] = "Some thing went wrong. Please try again!";

                return Redirect(FormData.ReturnURL);
            }

            //--Data Delete Part starts here
            FormData.resultMsg = AdminServices.Instance.DeleteTeacher(user_id);

            if (FormData.resultMsg == "Deleted Successfully!")
            {
                TempData["SuccessMsg"] = FormData.resultMsg;
                return Redirect(FormData.ReturnURL);
            }
            else
            {
                TempData["ErrorMsg"] = FormData.resultMsg;
                return Redirect(FormData.ReturnURL);
            }

        }

        [HttpGet]
        public ActionResult TeacherAssignCourses(int? pageId, int user_id, string title, int? course_id)
        {
            pageId = pageId == null || pageId == 0 ? 1 : pageId;
            course_id = course_id == null || course_id == 0 ? 0 : course_id;

            ViewBag.user_id = user_id;
            BasicDataModels model = new BasicDataModels();
            model.courses_obj = new courses();


            model.courses_list = AdminServices.Instance.GetTeacherAssignCourses(pageId, Constants.ITEMS_PER_PAGE, user_id, title, course_id).ToList();

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


            if (Request.IsAjaxRequest())
            {
                return PartialView("~/Areas/Admin/Views/AdminHome/_TeacherAssignCoursesPartial.cshtml", model);
            }


            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AssignNewCourseToTeacher(teacher_assign_courses FormData)
        {
            BasicDataModels model = new BasicDataModels();

            //--getting calling page URL
            FormData.ReturnURL = this.Request.UrlReferrer.PathAndQuery;


            if (FormData.user_id == null)
            {
                TempData["ErrorMsg"] = "user_id is null!";
                return Redirect(FormData.ReturnURL);
            }

            if (FormData.user_id < 1)
            {
                TempData["ErrorMsg"] = "user_id should not be in minus!";
                return Redirect(FormData.ReturnURL);
            }

            if (FormData.course_id < 1)
            {
                TempData["ErrorMsg"] = "Please fill all required fields!";
                return Redirect(FormData.ReturnURL);
            }

            //--Data update Part starts here
            FormData.created_by = SessionManager.user.user_id;
            FormData.resultMsg = AdminServices.Instance.AssignCourseToTeacher(FormData);
            if (FormData.resultMsg == "Saved Successfully!")
            {
                TempData["SuccessMsg"] = FormData.resultMsg;
                return Redirect(FormData.ReturnURL);
            }
            else
            {
                TempData["ErrorMsg"] = FormData.resultMsg;
                return Redirect(FormData.ReturnURL);
            }

        }

        [HttpGet]
        public JsonResult GetTeacherUnassignCourseList(int user_id, string course_title)

        {
            BasicDataModels model = new BasicDataModels();
            //---get status list
            model.courses_list = AdminServices.Instance.GetTeacherUnAssignCourseList(user_id, course_title).ToList();

            if (model.courses_list != null)
            {
                if (model.courses_list.Count() > 0)
                {
                    return Json(model.courses_list, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { message = "No data found" }, JsonRequestBehavior.AllowGet);
                }

            }
            else
            {
                return Json(new { message = "No data found" }, JsonRequestBehavior.AllowGet);
            }


        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RemoveCourseFromTeacherCourses(int teacher_assign_id)
        {
            courses FormData = new courses();


            //--getting calling page URL
            FormData.ReturnURL = this.Request.UrlReferrer.PathAndQuery;

            if (teacher_assign_id < 1)
            {
                TempData["ErrorMsg"] = "Some thing went wrong. Please try again!";

                return Redirect(FormData.ReturnURL);
            }

            //--Data Delete Part starts here
            FormData.resultMsg = AdminServices.Instance.RemoveCourseFromTeacherCoursesList(teacher_assign_id);

            if (FormData.resultMsg == "Deleted Successfully!")
            {
                TempData["SuccessMsg"] = FormData.resultMsg;
                return Redirect(FormData.ReturnURL);
            }
            else
            {
                TempData["ErrorMsg"] = FormData.resultMsg;
                return Redirect(FormData.ReturnURL);
            }

        }

        public JsonResult GetAdminFooterTotalStudentsCourses()
        {
            JsonResult result = new JsonResult();
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            FooterData model = new FooterData();

            model = AdminServices.Instance.GetAdminFooterTotalStudentsCourses();
            if (model != null)
            {
                result.Data = new { Success = true, FooterData = model, Message = "Successfully!" };


            }
            else
            {
                result.Data = new { Success = false, Message = "An error occured. Please try again!" };
            }

            return result;
        }
    }
}