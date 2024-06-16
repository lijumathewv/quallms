using Microsoft.AspNetCore.Mvc;
using QualLMS.Domain.APIModels;
using QualLMS.Domain.Models;
using QualvationLibrary;
using System.Text.Json;

namespace QualLMS.WebAppMvc.Controllers
{
    public class AttendanceController(Client client, CustomLogger logger) : Controller
    {
        public IActionResult Index()
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

            var data = client.ExecutePostAPI<List<AttendanceData>>("Attendance/list-all-attendance?Id=" + logger.LoginDetails.OrganizationId);

            return View(data);
        }

        [HttpPost("Checkin")]
        public IActionResult CheckIn()
        {
            DateTime utcNow = DateTime.UtcNow;
            TimeZoneInfo istTimeZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            DateTime istNow = TimeZoneInfo.ConvertTimeFromUtc(utcNow, istTimeZone);

            var model = new AttendanceData
            {
                AppId = logger.LoginDetails.Id,
                CurrentDate = istNow,
                CheckIn = istNow,
            };

            var data = client.ExecutePostAPI<ResultCommon>("Attendance/checkin", JsonSerializer.Serialize(model));

            TempData["IsError"] = data.Error;

            TempData["IsSuccess"] = !data.Error;

            return RedirectToAction("Index");
        }
    }
}
