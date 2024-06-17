using Microsoft.AspNetCore.Mvc;
using QualLMS.Domain.Contracts;
using QualLMS.Domain.Models;
using QualvationLibrary;
using System.Text.Json;

namespace QualLMS.WebAppMvc.Controllers
{
    public class CourseController(Client client, CustomLogger logger, ICourse repo, LoginProperties login) : Controller
    {
        [HttpGet("Course")]
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
            var response = repo.GetAll(login.OrganizationId.ToString());//client.ExecutePostAPI<List<Course>>("Course/all?OrgId=" + login.OrganizationId);
            var data = client.ParseResult<List<Course>>(response.returnmodel);

            return View(data);
        }

        [HttpPost("AddCourse")]
        public IActionResult AddCourseList(IFormCollection form)
        {
            if (login.Id == Guid.Empty)
            {
                login = JsonSerializer.Deserialize<LoginProperties>(HttpContext.Session.GetString("LoginDetails"));
            }

            Guid CourseId = Guid.Empty;

            if (!string.IsNullOrEmpty(form["CourseId"]))
            {
                CourseId = new Guid(form["CourseId"].ToString());
            }

            var model = new Course
            {
                Id = CourseId,
                OrganizationId = login.OrganizationId,
                CourseFees = Convert.ToInt32(form["CourseFees"].ToString()),
                CourseName = form["CourseName"].ToString()
            };
            var response = repo.AddOrUpdate(model);//client.ExecutePostAPI<ResultCommon>("Course/add", JsonSerializer.Serialize(model));
            //var data = client.ParseResult<ResultCommon>(response.returnmodel);
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
