using Azure;
using Microsoft.AspNetCore.Mvc;
using QualLMS.Domain.APIModels;
using QualLMS.Domain.Contracts;
using QualLMS.Domain.Models;
using QualvationLibrary;
using System.Text.Json;

namespace QualLMS.WebAppMvc.Controllers
{
    public class StudentCourseController(Client client, CustomLogger logger, IStudentCourse repo, ICourse course, IApplicationUserAccount user, LoginProperties login) : Controller
    {
        [HttpGet("StudentCourse")]
        public IActionResult Index()
        {
            if (login.Id == Guid.Empty)
            {
                login = JsonSerializer.Deserialize<LoginProperties>(HttpContext.Session.GetString("LoginDetails"));
            }

            if (TempData["IsError"] != null)
            {
                ViewBag.IsError = TempData["IsError"];
                ViewBag.ErrorMessage = logger.ErrorMessage;
            }
            if (TempData["IsSuccess"] != null)
            {
                ViewBag.IsSuccess = TempData["IsSuccess"];
                ViewBag.SuccessMessage = "Data updated successfully!";
            }

            var response = repo.GetAll(login.OrganizationId.ToString());//client.ExecutePostAPI<List<StudentCourseData>>("StudentCourse/all?OrgId=" + login.OrganizationId);

            var data = client.ParseResult<List<StudentCourseData>>(response.returnmodel);

            return View(data);
        }

        [HttpGet("AddStudentCourse")]
        public IActionResult AddStudentCourse(string Id)
        {
            if (login.Id == Guid.Empty)
            {
                login = JsonSerializer.Deserialize<LoginProperties>(HttpContext.Session.GetString("LoginDetails"));
            }

            if (TempData["IsError"] != null)
            {
                ViewBag.IsError = TempData["IsError"];
                ViewBag.ErrorMessage = logger.ErrorMessage;
            }

            if (TempData["IsSuccess"] != null)
            {
                ViewBag.IsSuccess = TempData["IsSuccess"];
                ViewBag.SuccessMessage = "Data updated successfully!";
            }

            AddStudentCourseData Model = new AddStudentCourseData();

            Model.Data = new StudentCourseData();
            var response = course.GetAll(login.OrganizationId.ToString());
            Model.Courses = client.ParseResult<List<Course>>(response.returnmodel);
            response = user.GetStudents(login.OrganizationId.ToString());
            Model.Users = client.ParseResult<List<UserAllData>>(response.returnmodel);

            if (!string.IsNullOrEmpty(Id))
            {
                response = repo.Get(Id);//client.ExecutePostAPI<StudentCourseData>("StudentCourse/getsingle?Id=" + Id);
                Model.Data = client.ParseResult<StudentCourseData>(response.returnmodel);

            }

            return View(Model);
        }

        [HttpPost("AddStudentCourse")]
        public IActionResult AddStudentCourse(StudentCourseData Model)
        {
            if (login.Id == Guid.Empty)
            {
                login = JsonSerializer.Deserialize<LoginProperties>(HttpContext.Session.GetString("LoginDetails"));
            }

            Model.OrganizationId = login!.OrganizationId;

            if (Model.StudentId == Guid.Empty)
            {
                Model.StudentId = new Guid(Request.Form["Data.StudentId"]);
            }
            if (Model.CourseId == Guid.Empty)
            {
                Model.CourseId = new Guid(Request.Form["Data.CourseId"]);
            }

            var response = repo.AddOrUpdate(Model); //client.ExecutePostAPI<ResultCommon>("StudentCourse/add", JsonSerializer.Serialize(Model));

            TempData["IsSuccess"] = response.flag;

            if (!response.flag)
            {
                TempData["IsError"] = true;
                logger.ErrorMessage = response.message;
            }

            return RedirectToActionPermanent("Index");
        }
    }
}
