using Microsoft.AspNetCore.Mvc;
using QualLMS.Domain.Models;
using QualvationLibrary;
using System.Text.Json;

namespace QualLMS.WebAppMvc.Controllers
{
    public class MasterController(Client client, CustomLogger logger) : Controller
    {
        [HttpGet("Course")]
        public IActionResult CourseList()
        {
            if (logger.IsLogged)
            {
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

                var data = client.ExecutePostAPI<List<Course>>("Course/all");

                return View(data);
            }
            else
            {
                return RedirectToActionPermanent("Index", "Login");
            }
        }

        [HttpPost("AddCourse")]
        public IActionResult AddCourseList(IFormCollection form)
        {
            if (logger.IsLogged)
            {
                Guid CourseId = Guid.Empty;

                if (!string.IsNullOrEmpty(form["CourseId"])) {
                    CourseId = new Guid(form["CourseId"].ToString());
                }

                var model = new Course
                {
                    Id = CourseId,
                    OrganizationId = logger.LoginDetails.OrganizationId,
                    CourseFees = Convert.ToInt32(form["CourseFees"].ToString()),
                    CourseName = form["CourseName"].ToString()
                };

                var data = client.ExecutePostAPI<ResultCommon>("Course/add", JsonSerializer.Serialize(model));

                TempData["IsError"] = data.Error;

                TempData["IsSuccess"] = !data.Error;

                return RedirectToActionPermanent("CourseList");
            }
            else
            {
                return RedirectToActionPermanent("Index", "Login");
            }
        }
    }
}
