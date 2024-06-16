using Microsoft.AspNetCore.Mvc;
using QualLMS.Domain.APIModels;
using QualLMS.Domain.Models;
using QualvationLibrary;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace QualLMS.WebAppMvc.Controllers
{
    public class TransactionController(Client client, CustomLogger logger) : Controller
    {
        [HttpGet("StudentCourse")]
        public IActionResult StudentCourseList()
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

                var data = client.ExecutePostAPI<List<StudentCourseData>>("StudentCourse/all?OrgId=" + logger.LoginDetails.OrganizationId);

                return View(data);
            }
            else
            {
                return RedirectToActionPermanent("Index", "Login");
            }
        }

        [HttpGet("AddStudentCourse")]
        public IActionResult AddStudentCourse(string Id)
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

                if (!string.IsNullOrEmpty(Id))
                {
                    var data = client.ExecutePostAPI<StudentCourseData>("StudentCourse/getsingle?Id=" + Id);

                    return View(data);
                }
                else
                {
                    return View(new StudentCourseData());
                }

            }
            else
            {
                return RedirectToActionPermanent("Index", "Login");
            }
        }

        [HttpPost("AddStudentCourse")]
        public IActionResult AddStudentCourse(StudentCourseData Model)
        {
            if (logger.IsLogged)
            {
                Model.OrganizationId = logger.LoginDetails!.OrganizationId;

                var data = client.ExecutePostAPI<ResultCommon>("StudentCourse/add", JsonSerializer.Serialize(Model));

                if (data.Error)
                {
                    ViewBag.IsError = data.Error;
                    ViewBag.ErrorMessage = logger.ErrorMessage;
                }
                if (!data.Error)
                {
                    ViewBag.IsSuccess = !data.Error;
                    ViewBag.SuccessMessage = "Data updated successfully!";
                }

                if (data.Error)
                {
                    return View(Model);
                }
                else
                {
                    return View(new StudentCourseData());
                }

            }
            else
            {
                return RedirectToActionPermanent("Index", "Login");
            }
        }

        [HttpGet("Receipts")]
        public IActionResult FeesReceived()
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

                var data = client.ExecutePostAPI<List<FeesReceivedData>>("FeesReceived/all");

                return View(data);
            }
            else
            {
                return RedirectToActionPermanent("Index", "Login");
            }
        }

        [HttpGet("AddReceipts")]
        public IActionResult AddFeesReceived(string Id)
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

                if (!string.IsNullOrEmpty(Id))
                {
                    var data = client.ExecutePostAPI<FeesReceivedData>("FeesReceived/getsingle?Id=" + Id);

                    return View(data);
                }
                else
                {
                    DateOnly date;
                    DateOnly.TryParse(DateTime.Today.ToString(), out date);

                    return View(new FeesReceivedData() { ReceiptDate = date });
                }

            }
            else
            {
                return RedirectToActionPermanent("Index", "Login");
            }
        }

        [HttpPost("AddReceipts")]
        public IActionResult AddFeesReceived(FeesReceivedData Model)
        {
            if (logger.IsLogged)
            {
                Model.OrganizationId = logger.LoginDetails.OrganizationId;
                //Model.UserId = logger.LoginDetails.Id;
                Model.Mode = PaymentMode.Cash;
                Model.ModeDetails = "Received in Cash";

                var data = client.ExecutePostAPI<ResultCommon>("FeesReceived/add", JsonSerializer.Serialize(Model));

                if (data.Error)
                {
                    ViewBag.IsError = data.Error;
                    ViewBag.ErrorMessage = data.Message;
                    if (data.ApiError != null)
                    {
                        ViewBag.ErrorMessage += "<br/>" + logger.ParseAPIError(data.ApiError);
                    }
                }
                if (!data.Error)
                {
                    ViewBag.IsSuccess = !data.Error;
                    ViewBag.SuccessMessage = "Data updated successfully!";
                }

                if (data.Error)
                {
                    return View(Model);
                }
                else
                {
                    return View(new FeesReceivedData());
                }

            }
            else
            {
                return RedirectToActionPermanent("Index", "Login");
            }
        }

        [HttpGet("TeacherCalendar")]
        public IActionResult TeacherCalendarList()
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

                var data = client.ExecutePostAPI<List<CalendarData>>("Calendar/all?OrgId=" + logger.LoginDetails.OrganizationId);

                return View(data);
            }
            else
            {
                return RedirectToActionPermanent("Index", "Login");
            }
        }

        [HttpPost("AddCalendar")]
        public IActionResult AddTeacherCalendar(IFormCollection form)
        {
            if (logger.IsLogged)
            {
                string TeacherId = string.Empty;
                Guid CourseId = Guid.Empty;
                Guid CalendarId = Guid.Empty;
                if (!string.IsNullOrEmpty(form["CalendarId"]))
                {
                    CalendarId = new Guid(form["CalendarId"].ToString());
                }

                if (!string.IsNullOrEmpty(form["TeacherId"]))
                {
                    TeacherId = form["TeacherId"].ToString();
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
                    var model = new CalendarData
                    {
                        Id = CalendarId,
                        TeacherId = TeacherId,
                        CourseId = CourseId,
                        OrganizationId = logger.LoginDetails.OrganizationId,
                        Date = DateOnly.Parse(form["Date"].ToString()),
                        StartTime = TimeOnly.Parse(form["StartTime"].ToString()),
                        EndTime = TimeOnly.Parse(form["EndTime"].ToString())
                    };

                    var data = client.ExecutePostAPI<ResultCommon>("Calendar/add", JsonSerializer.Serialize(model));

                    TempData["IsError"] = data.Error;
                    TempData["IsSuccess"] = !data.Error;
                }
                else
                {
                    TempData["IsError"] = logger.IsError;
                    TempData["IsSuccess"] = !logger.IsError;
                }

                return RedirectToActionPermanent("TeacherCalendarList");
            }
            else
            {
                return RedirectToActionPermanent("Index", "Login");
            }
        }
    }
}
