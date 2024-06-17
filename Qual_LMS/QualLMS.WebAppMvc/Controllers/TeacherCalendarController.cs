using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using QualLMS.Domain.APIModels;
using QualLMS.Domain.Contracts;
using QualLMS.Domain.Models;
using QualvationLibrary;
using System.Text.Json;

namespace QualLMS.WebAppMvc.Controllers
{
    public class TeacherCalendarController(Client client, CustomLogger logger, ICalendar repo, IApplicationUserAccount user, ICourse course, LoginProperties login) : Controller
    {
        [HttpGet("TeacherCalendar")]
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

            TeacherCalendarData Model = new TeacherCalendarData();

            var response = repo.GetAll(login!.OrganizationId.ToString());//client.ExecutePostAPI<List<CalendarData>>("Calendar/all?OrgId=" + login.OrganizationId)

            Model.Data = JsonSerializer.Serialize(client.ParseResult<List<CalendarData>>(response.returnmodel));

            response = user.GetTeachers(login!.OrganizationId.ToString());// client.ExecutePostAPI<List<UserAllData>>("Account/allteachers?OrgId=" + login!.OrganizationId);
            Model.Users = client.ParseResult<List<UserAllData>>(response.returnmodel);
            
            response = response = course.GetAll(login!.OrganizationId.ToString());//client.ExecutePostAPI<List<Course>>("Course/all?OrgId=" + login.OrganizationId);
            Model.Courses = client.ParseResult<List<Course>>(response.returnmodel); 

            return View(Model);
        }


        [HttpPost("AddCalendar")]
        public IActionResult AddTeacherCalendar(IFormCollection form)
        {
            if (login.Id == Guid.Empty)
            {
                login = JsonSerializer.Deserialize<LoginProperties>(HttpContext.Session.GetString("LoginDetails"));
            }

            Guid TeacherId = Guid.Empty;
            Guid CourseId = Guid.Empty;
            Guid CalendarId = Guid.Empty;
            if (!string.IsNullOrEmpty(form["CalendarId"]))
            {
                CalendarId = new Guid(form["CalendarId"].ToString());
            }

            if (!string.IsNullOrEmpty(form["TeacherId"]))
            {
                TeacherId = new Guid(form["TeacherId"].ToString());
            }
            else
            {
                logger.IsError = true;
                logger.ErrorMessage = "Teacher not selected!";
            }


            if (!string.IsNullOrEmpty(form["CourseId"]))
            {
                CourseId = new Guid(form["CourseId"].ToString());
            }
            else
            {
                logger.IsError = true;
                logger.ErrorMessage += "<br/>Course not selected!";
            }

            if (!logger.IsError)
            {
                var Model = new CalendarData
                {
                    Id = CalendarId,
                    TeacherId = TeacherId,
                    CourseId = CourseId,
                    OrganizationId = login.OrganizationId,
                    Date = DateOnly.Parse(form["Date"].ToString()),
                    StartTime = TimeOnly.Parse(form["StartTime"].ToString()),
                    EndTime = TimeOnly.Parse(form["EndTime"].ToString())
                };
                var response = repo.AddOrUpdate(Model); //client.ExecutePostAPI<ResultCommon>("Calendar/add", JsonSerializer.Serialize(model));

                TempData["IsSuccess"] = response.flag;

                if (!response.flag)
                {
                    TempData["IsError"] = true;
                    logger.ErrorMessage = response.message;
                }
            }

            return RedirectToActionPermanent("Index");
        }
    }
}
