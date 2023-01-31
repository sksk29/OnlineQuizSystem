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
    [AdminAuthorization]
    public class AdminBasicDataController : AdminBaseController
    {
        // GET: Admin/AdminBasicData
        [HttpGet]
        public ActionResult QuizCategories(int? pageId, string category_name, string is_active)
        {


            pageId = pageId== null || pageId==0 ? 1 : pageId;

            BasicDataModels model = new BasicDataModels();
            model.quiz_categories_obj = new quiz_categories();

            model.quiz_categories_list = AdminServices.Instance.GetQuizCategoriesList(pageId,Constants.ITEMS_PER_PAGE,  category_name,  is_active).ToList();

            model.quiz_categories_obj.pageId=Convert.ToInt32(pageId);
            if (model.quiz_categories_list.Count()>0)
            {
                model.quiz_categories_obj.TotalRecordCount = (int)model.quiz_categories_list.FirstOrDefault().TotalRecordCount;
            }
            else
            {
                model.quiz_categories_obj.TotalRecordCount = 0;
            }
          
            model.quiz_categories_obj.ItemsPerPage= Constants.ITEMS_PER_PAGE;

            if (Request.IsAjaxRequest())
            {
                return PartialView("~/Areas/Admin/Views/AdminBasicData/_QuizCategoriesPartial.cshtml", model);
            }


            return View(model);
        }

     
        [HttpPost]
        public ActionResult QuizCategories(quiz_categories FormData)
        {
            BasicDataModels model = new BasicDataModels();

            //--getting calling page URL
            FormData.ReturnURL = this.Request.UrlReferrer.PathAndQuery;

            if (String.IsNullOrEmpty(FormData.category_name))
            {
                TempData["ErrorMsg"]= "Please fill all required fields!";
                FormData.ErrorMsg = "Please fill all required fields!";
                return Redirect(FormData.ReturnURL);
            }

            //--Data Insert Part starts here
            FormData.created_by = SessionManager.user.user_id;
            FormData.ErrorMsg = AdminServices.Instance.InsertQuizCategory(FormData);
            if (FormData.ErrorMsg=="Saved Successfully!")
            {
                TempData["SuccessMsg"] = FormData.ErrorMsg;
                return Redirect(FormData.ReturnURL);
            }
            else
            {
                TempData["ErrorMsg"] = FormData.ErrorMsg;
                return Redirect(FormData.ReturnURL);
            }

            return Redirect(FormData.ReturnURL);
        }

        [HttpPost]
        public ActionResult QuizCategoriesEdit(quiz_categories FormData)
        {
          

            //--getting calling page URL
            FormData.ReturnURL = this.Request.UrlReferrer.PathAndQuery;

            if (String.IsNullOrEmpty(FormData.category_name) || FormData.category_id<1)
            {
                TempData["ErrorMsg"] = "Please fill all required fields!";

                return Redirect(FormData.ReturnURL);
            }

            //--Data Insert Part starts here
            FormData.modified_by = SessionManager.user.user_id;
            FormData.ErrorMsg = AdminServices.Instance.UpdateQuizCategory(FormData);
            if (FormData.ErrorMsg == "Updated Successfully!")
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


        [HttpPost]
        public ActionResult DeleteQuizCategories(quiz_categories FormData)
        {


            //--getting calling page URL
            FormData.ReturnURL = this.Request.UrlReferrer.PathAndQuery;

            if (FormData.category_id < 1)
            {
                TempData["ErrorMsg"] = "Some thing went wrong. Please try again!";

                return Redirect(FormData.ReturnURL);
            }

            //--Data Delete Part starts here
            FormData.ErrorMsg = AdminServices.Instance.DeleteQuizCategory(FormData);

            if (FormData.ErrorMsg == "Deleted Successfully!")
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
        public ActionResult CourseCategories(int? pageId, string title)
        {


            pageId = pageId == null || pageId == 0 ? 1 : pageId;

            BasicDataModels model = new BasicDataModels();
            model.course_categories_obj = new course_categories();

            model.course_categories_list = AdminServices.Instance.GetCourseCategoriesList(pageId, Constants.ITEMS_PER_PAGE, title).ToList();

            model.course_categories_obj.pageId = Convert.ToInt32(pageId);
            if (model.course_categories_list.Count() > 0)
            {
                model.course_categories_obj.TotalRecordCount = (int)model.course_categories_list.FirstOrDefault().TotalRecordCount;
            }
            else
            {
                model.course_categories_obj.TotalRecordCount = 0;
            }

            model.course_categories_obj.ItemsPerPage = Constants.ITEMS_PER_PAGE;

            if (Request.IsAjaxRequest())
            {
                return PartialView("~/Areas/Admin/Views/AdminBasicData/_CourseCategoriesPartial.cshtml", model);
            }


            return View(model);
        }

      
        [HttpPost]
        [AdminAuthorization]
        [ValidateAntiForgeryToken]
        public ActionResult CourseCategories(course_categories FormData)
        {
            BasicDataModels model = new BasicDataModels();

            //--getting calling page URL
            FormData.ReturnURL = this.Request.UrlReferrer.PathAndQuery;

            if (String.IsNullOrEmpty(FormData.title))
            {
                TempData["ErrorMsg"] = "Please fill all required fields!";
                FormData.ErrorMsg = "Please fill all required fields!";

                model.course_categories_obj = FormData;
                // return View(model);
                return Redirect(FormData.ReturnURL);
            }

            //--Data Insert Part starts here
            FormData.created_by = SessionManager.user.user_id;
            FormData.guid = CommonHelper.Instance.GeneratNewGUID();
            FormData.ErrorMsg = AdminServices.Instance.InsertCourseCategory(FormData);
            if (FormData.ErrorMsg == "Saved Successfully!")
            {
                TempData["SuccessMsg"] = FormData.ErrorMsg;
            }
            else
            {
                TempData["ErrorMsg"] = FormData.ErrorMsg;
            }

            model.course_categories_obj = FormData;

            //return View(model);
            return Redirect(FormData.ReturnURL);
        }

      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditCourseCategory(course_categories FormData)
        {


            //--getting calling page URL
            FormData.ReturnURL = this.Request.UrlReferrer.PathAndQuery;

            if (String.IsNullOrEmpty(FormData.title) || FormData.course_category_id < 1)
            {
                TempData["ErrorMsg"] = "Please fill all required fields!";

                return Redirect(FormData.ReturnURL);
            }

            //--Data Insert Part starts here
            FormData.modified_by = SessionManager.user.user_id;
            FormData.ErrorMsg = AdminServices.Instance.UpdateCourseCategory(FormData);
            if (FormData.ErrorMsg == "Updated Successfully!")
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

     
        [HttpPost]
        [ValidateAntiForgeryToken]
      
        public ActionResult DeleteCourseCategory(course_categories FormData)
        {


            //--getting calling page URL
            FormData.ReturnURL = this.Request.UrlReferrer.PathAndQuery;

            if (FormData.course_category_id < 1)
            {
                TempData["ErrorMsg"] = "Some thing went wrong. Please try again!";

                return Redirect(FormData.ReturnURL);
            }

            //--Data Delete Part starts here
            FormData.ErrorMsg = AdminServices.Instance.DeleteCourseCategory(FormData);

            if (FormData.ErrorMsg == "Deleted Successfully!")
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
     



     
    }
}