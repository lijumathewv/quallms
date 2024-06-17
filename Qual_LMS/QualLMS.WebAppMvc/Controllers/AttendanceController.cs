using Microsoft.AspNetCore.Mvc;
using QualLMS.Domain.APIModels;
using QualLMS.Domain.Contracts;
using QualLMS.Domain.Models;
using QualvationLibrary;
using System.Text.Json;

namespace QualLMS.WebAppMvc.Controllers
{
    public class AttendanceController(ILogger<HomeController> _logger, Client client, CustomLogger logger, IAttendance repo) : BaseController
    {
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
                var response = repo.GetMyAttendance(new Guid(GetSessionValue("LoginId")));//client.ExecutePostAPI<List<AttendanceData>>("Attendance/list-all-attendance?Id=" + new Guid(GetSessionValue("OrganizationId")));

                ViewAttendanceData Model = new ViewAttendanceData();

                var data = client.ParseResult<List<AttendanceData>>(response.returnmodel);

                Model.Data = data;
                Model.IsCheckedIn = IsCheckedIn();
                Model.IsCheckedOut = IsCheckedOut();

                return View(Model);
            }
            catch (Exception ex)
            {
                logger.GenerateException(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("Checkin")]
        public string CheckIn()
        {
            
            DateTime utcNow = DateTime.UtcNow;
            TimeZoneInfo istTimeZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            DateTime istNow = TimeZoneInfo.ConvertTimeFromUtc(utcNow, istTimeZone);

            var model = new AttendanceData
            {
                AppId = new Guid(GetSessionValue("LoginId")),
                CurrentDate = new DateOnly(istNow.Date.Year, istNow.Date.Month, istNow.Date.Day),
                CheckIn = istNow,
            };

            var response = repo.CheckIn(model);//client.ExecutePostAPI<ResultCommon>("Attendance/checkin", JsonSerializer.Serialize(model));

            return JsonSerializer.Serialize(response);
        }

        [HttpPost("CheckOut")]
        public string CheckOut()
        {
            DateTime utcNow = DateTime.UtcNow;
            TimeZoneInfo istTimeZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            DateTime istNow = TimeZoneInfo.ConvertTimeFromUtc(utcNow, istTimeZone);

            var model = new AttendanceData
            {
                AppId = new Guid(GetSessionValue("LoginId")),
                CurrentDate = new DateOnly(istNow.Date.Year, istNow.Date.Month, istNow.Date.Day),
                CheckOut = istNow,
            };

            var response = repo.CheckOut(model);//client.ExecutePostAPI<ResultCommon>("Attendance/checkin", JsonSerializer.Serialize(model));

            return JsonSerializer.Serialize(response);
        }

        private bool IsCheckedIn()
        {
            var response = repo.GetAttendanceForToday(new Guid(GetSessionValue("LoginId")));

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
            var response = repo.GetAttendanceForToday(new Guid(GetSessionValue("LoginId")));

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
