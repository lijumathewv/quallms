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

                var data = client.ExecutePostAPI<List<StudentCourseData>>("StudentCourse/all?OrgId=" + logger.LoginDetails.OrganizationId);

                return View(data);
            }
            catch (Exception ex)
            {
                logger.GenerateException(ex);
                return View(new List<StudentCourseData>());
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
            catch (Exception ex)
            {
                ViewBag.IsError = TempData["IsError"];
                ViewBag.ErrorMessage = logger.ErrorMessage;
                logger.GenerateException(ex);
                return View(new StudentCourseData());
            }
        }

        [HttpPost("AddStudentCourse")]
        public IActionResult AddStudentCourse(StudentCourseData Model)
        {
            try
            {
                Model.OrganizationId = logger.LoginDetails!.OrganizationId;

                var data = client.ExecutePostAPI<ResultCommon>("StudentCourse/add", JsonSerializer.Serialize(Model));

                if (data.Error)
                {
                    throw new Exception(data.Message);
                }

                ViewBag.IsSuccess = !data.Error;
                ViewBag.SuccessMessage = "Data updated successfully!";

                return RedirectToActionPermanent("StudentCourseList");
            }
            catch (Exception ex)
            {
                ViewBag.IsError = true;
                ViewBag.ErrorMessage = logger.ErrorMessage;
                logger.GenerateException(ex);
                return View(Model);
            }
        }

        [HttpGet("Receipts")]
        public IActionResult FeesReceived()
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

                var data = client.ExecutePostAPI<List<FeesReceivedData>>("FeesReceived/all");

                return View(data);
            }
            catch (Exception ex)
            {
                logger.GenerateException(ex);
                return View(new List<FeesReceivedData>());
            }
        }

        [HttpGet("AddReceipts")]
        public IActionResult AddFeesReceived(string Id)
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
            catch (Exception ex)
            {
                logger.GenerateException(ex);
                return View(new FeesReceivedData());
            }
        }

        [HttpPost("AddReceipts")]
        public IActionResult AddFeesReceived(FeesReceivedData Model)
        {
            try
            {
                Model.OrganizationId = logger.LoginDetails.OrganizationId;

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
                    throw new Exception(ViewBag.ErrorMessage);
                }
                if (!data.Error)
                {
                    ViewBag.IsSuccess = !data.Error;
                    ViewBag.SuccessMessage = "Data updated successfully!";
                }

                return View(new FeesReceivedData());
            }
            catch (Exception ex)
            {
                ViewBag.IsError = true;
                ViewBag.ErrorMessage = ex.Message;
                logger.GenerateException(ex);
                return View(Model);
            }
        }

        [HttpGet("TeacherCalendar")]
        public IActionResult TeacherCalendarList()
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

                var data = client.ExecutePostAPI<List<CalendarData>>("Calendar/all?OrgId=" + logger.LoginDetails.OrganizationId);

                return View(data);
            }
            catch (Exception ex)
            {
                ViewBag.IsError = true;
                ViewBag.ErrorMessage = ex.Message;

                logger.GenerateException(ex);
                return View(new List<CalendarData>());
            }
        }

        [HttpPost("AddCalendar")]
        public IActionResult AddTeacherCalendar(IFormCollection form)
        {
            try
            {
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
                    throw new Exception(logger.ErrorMessage);
                }

                return RedirectToActionPermanent("TeacherCalendarList");
            }
            catch (Exception ex)
            {
                TempData["IsError"] = true;
                TempData["IsSuccess"] = false;
                logger.ErrorMessage = ex.Message;

                logger.GenerateException(ex);
                return RedirectToActionPermanent("TeacherCalendarList");
            }
        }
    }
}
