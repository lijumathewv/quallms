using Azure;
using Microsoft.AspNetCore.Mvc;
using QualLMS.Domain.APIModels;
using QualLMS.Domain.Contracts;
using QualLMS.Domain.Models;
using QualvationLibrary;
using System.Text.Json;
using static QualvationLibrary.ServiceResponse;

namespace QualLMS.WebAppMvc.Controllers
{
    public class StudentCourseController(Client client, CustomLogger logger, IStudentCourse repo, ICourse course, IApplicationUserAccount user) : BaseController
    {
        [HttpGet("StudentCourse")]
        public IActionResult Index()
        {
            try
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

                var response = repo.GetAll(new Guid(GetSessionValue("OrganizationId")).ToString());//client.ExecutePostAPI<List<StudentCourseData>>("StudentCourse/all?OrgId=" + new Guid(GetSessionValue("OrganizationId")));

                var data = client.ParseResult<List<StudentCourseData>>(response.returnmodel);

                return View(data);
            }
            catch (Exception ex)
            {
                logger.GenerateException(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("AddStudentCourse")]
        public IActionResult AddStudentCourse(string Id)
        {
            try
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

                AddStudentCourseData Model = new AddStudentCourseData();

                Model.Data = new StudentCourseData();
                var response = course.GetAll(new Guid(GetSessionValue("OrganizationId")).ToString());
                Model.Courses = client.ParseResult<List<Course>>(response.returnmodel);
                response = user.GetStudents(new Guid(GetSessionValue("OrganizationId")).ToString());
                Model.Users = client.ParseResult<List<UserAllData>>(response.returnmodel);

                if (!string.IsNullOrEmpty(Id))
                {
                    response = repo.Get(Id);//client.ExecutePostAPI<StudentCourseData>("StudentCourse/getsingle?Id=" + Id);
                    Model.Data = client.ParseResult<StudentCourseData>(response.returnmodel);

                }

                return View(Model);
            }
            catch (Exception ex)
            {
                logger.GenerateException(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("AddStudentCourse")]
        public IActionResult AddStudentCourse(StudentCourseData Model)
        {
            try
            {
                Model.OrganizationId = new Guid(GetSessionValue("OrganizationId"));

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
            catch (Exception ex)
            {
                logger.GenerateException(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("GetFees")]
        public string GetCourseFees(string CourseId)
        {
            var response = course.GetFees(CourseId);
            return response.returnmodel;
        }
    }
}
