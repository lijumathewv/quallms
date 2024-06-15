using Microsoft.AspNetCore.Mvc;
using QualLMS.Domain.APIModels;
using QualLMS.Domain.Models;
using QualvationLibrary;
using System.Text.Json;

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

                var data = client.ExecutePostAPI<List<StudentCourseData>>("StudentCourse/all");

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
    }
}
