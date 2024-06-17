using Microsoft.AspNetCore.Mvc;
using QualLMS.Domain.Contracts;
using QualLMS.Domain.Models;
using QualvationLibrary;
using System.Text.Json;

namespace QualLMS.WebAppMvc.Controllers
{
    public class CourseController(Client client, CustomLogger logger, ICourse repo) : BaseController
    {
        [HttpGet("Course")]
        public IActionResult Index()
        {
            try
            {
                string OrganizationId = GetSessionValue("OrganizationId");

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
                var response = repo.GetAll(OrganizationId);//client.ExecutePostAPI<List<Course>>("Course/all?OrgId=" + new Guid(GetSessionValue("OrganizationId")));
                var data = client.ParseResult<List<Course>>(response.returnmodel);

                return View(data);
            }
            catch (Exception ex)
            {
                logger.GenerateException(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("AddCourse")]
        public IActionResult AddCourseList(IFormCollection form)
        {
            try
            {
                string OrganizationId = GetSessionValue("OrganizationId");

                Guid CourseId = Guid.Empty;

                if (!string.IsNullOrEmpty(form["CourseId"]))
                {
                    CourseId = new Guid(form["CourseId"].ToString());
                }

                var model = new Course
                {
                    Id = CourseId,
                    OrganizationId = new Guid(OrganizationId),
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
            catch (Exception ex)
            {
                logger.GenerateException(ex);
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
