using Microsoft.AspNetCore.Mvc;
using QualLMS.Domain.APIModels;
using QualLMS.Domain.Contracts;
using QualLMS.Domain.Models;
using QualvationLibrary;
using System.Text.Json;

namespace QualLMS.WebAppMvc.Controllers
{
    public class AttendanceController(Client client, CustomLogger logger, LoginProperties login, IAttendance repo) : Controller
    {
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
            var response = repo.GetMyAttendance(login.Id);//client.ExecutePostAPI<List<AttendanceData>>("Attendance/list-all-attendance?Id=" + login.OrganizationId);
            
            ViewAttendanceData Model = new ViewAttendanceData();

            var data = client.ParseResult<List<AttendanceData>>(response.returnmodel);

            Model.Data = data;
            Model.IsCheckedIn = IsCheckedIn();
            Model.IsCheckedOut = IsCheckedOut();

            return View(Model);
        }

        [HttpPost("Checkin")]
        public string CheckIn()
        {
            if (login.Id == Guid.Empty)
            {
                login = JsonSerializer.Deserialize<LoginProperties>(HttpContext.Session.GetString("LoginDetails"));
            }

            DateTime utcNow = DateTime.UtcNow;
            TimeZoneInfo istTimeZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            DateTime istNow = TimeZoneInfo.ConvertTimeFromUtc(utcNow, istTimeZone);

            var model = new AttendanceData
            {
                AppId = login.Id,
                CurrentDate = new DateOnly(istNow.Date.Year, istNow.Date.Month, istNow.Date.Day),
                CheckIn = istNow,
            };

            var response = repo.CheckIn(model);//client.ExecutePostAPI<ResultCommon>("Attendance/checkin", JsonSerializer.Serialize(model));

            return JsonSerializer.Serialize(response);
        }

        [HttpPost("CheckOut")]
        public string CheckOut()
        {
            if (login.Id == Guid.Empty)
            {
                login = JsonSerializer.Deserialize<LoginProperties>(HttpContext.Session.GetString("LoginDetails"));
            }

            DateTime utcNow = DateTime.UtcNow;
            TimeZoneInfo istTimeZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            DateTime istNow = TimeZoneInfo.ConvertTimeFromUtc(utcNow, istTimeZone);

            var model = new AttendanceData
            {
                AppId = login.Id,
                CurrentDate = new DateOnly(istNow.Date.Year, istNow.Date.Month, istNow.Date.Day),
                CheckOut = istNow,
            };

            var response = repo.CheckOut(model);//client.ExecutePostAPI<ResultCommon>("Attendance/checkin", JsonSerializer.Serialize(model));

            return JsonSerializer.Serialize(response);
        }

        private bool IsCheckedIn()
        {
            if (login.Id == Guid.Empty)
            {
                login = JsonSerializer.Deserialize<LoginProperties>(HttpContext.Session.GetString("LoginDetails"));
            }

            var response = repo.GetAttendanceForToday(login.Id);

            if (response == null)
            {
                return false;
            }
            else
            {
                if (response.returnmodel == null)
                {
                    return false;
                }
                return true;
            }

        }
        
        private bool IsCheckedOut()
        {
            if (login.Id == Guid.Empty)
            {
                login = JsonSerializer.Deserialize<LoginProperties>(HttpContext.Session.GetString("LoginDetails"));
            }

            var response = repo.GetAttendanceForToday(login.Id);

            if (response == null)
            {
                return false;
            }
            else
            {
                if (response.returnmodel == null)
                {
                    return false;
                }
                var data = JsonSerializer.Deserialize<Attendance>(response.returnmodel);
                if (data == null)
                {
                    return false;
                }
                else
                {
                    if (data.CheckOut == null)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
