using Azure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using QualLMS.Domain.APIModels;
using QualLMS.Domain.Contracts;
using QualLMS.Domain.Models;
using QualvationLibrary;
using System.Text.Json;
using static QualvationLibrary.ServiceResponse;

namespace QualLMS.WebAppMvc.Controllers
{
    public class ReceiptsController(Client client, CustomLogger logger, IFeesReceived repo, IApplicationUserAccount user, IStudentCourse studentCourse, IFees fees) : BaseController
    {
        [HttpGet("Receipts")]
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

                var response = repo.GetAll(new Guid(GetSessionValue("OrganizationId")).ToString());// client.ExecutePostAPI<List<FeesReceivedData>>("FeesReceived/all");

                return View(client.ParseResult<List<FeesReceivedData>>(response.returnmodel));
            }
            catch (Exception ex)
            {
                logger.GenerateException(ex);
                return StatusCode(500, "Internal server error");
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

                AddFeesReceivedData Model = new AddFeesReceivedData();
                Model.ReceiptDate = new DateOnly(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

                ResponsesWithData response;
                if (!string.IsNullOrEmpty(Id))
                {
                    response = repo.GetAll(new Guid(GetSessionValue("OrganizationId")).ToString());//client.ExecutePostAPI<FeesReceivedData>("FeesReceived/getsingle?Id=" + Id);
                    Model = client.ParseResult<AddFeesReceivedData>(response.returnmodel);
                }

                response = user.GetStudents(new Guid(GetSessionValue("OrganizationId")).ToString());//client.ExecutePostAPI<List<UserAllData>>("Account/allstudents?OrgId=" + login!.OrganizationId);
                Model.Students = client.ParseResult<List<UserAllData>>(response.returnmodel);

                if (Model.StudentId != Guid.Empty)
                {
                    response = studentCourse.GetStudentCourse(Model.StudentId.ToString());//client.ExecutePostAPI<List<StudentCourse>>("StudentCourse/studentcourse?StudentId=" + Model.StudentId);
                    Model.Courses = client.ParseResult<List<StudentCourse>>(response.returnmodel);
                }
                else
                {
                    Model.Courses = new List<StudentCourse>();
                }

                response = fees.GetAll(new Guid(GetSessionValue("OrganizationId")).ToString()); //client.ExecutePostAPI<List<Fees>>("Fees/all?OrgId=" + new Guid(GetSessionValue("OrganizationId")));
                Model.Fees = client.ParseResult<List<Fees>>(response.returnmodel);

                return View(Model);
            }
            catch (Exception ex)
            {
                logger.GenerateException(ex);
                return StatusCode(500, "Internal server error");
            }

        }

        [HttpPost("AddReceipts")]
        public IActionResult AddFeesReceived(AddFeesReceivedData Model)
        {
            try
            {
                Model.OrganizationId = new Guid(GetSessionValue("OrganizationId"));

                Model.Mode = PaymentMode.Cash;
                Model.ModeDetails = "Received in Cash";

                var data = repo.AddOrUpdate(Model);// client.ExecutePostAPI<ResultCommon>("FeesReceived/add", JsonSerializer.Serialize(Model));

                if (!data.flag)
                {
                    ViewBag.IsError = true;
                    ViewBag.ErrorMessage = data.message;
                    //if (data.ApiError != null)
                    //{
                    //    ViewBag.ErrorMessage += "<br/>" + logger.ParseAPIError(data.ApiError);
                    //}
                    return View(Model);
                }

                ViewBag.IsSuccess = true;
                ViewBag.SuccessMessage = "Data updated successfully!";

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                logger.GenerateException(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("studentcourses")]
        public ResponsesWithData StudentCourses(string StudentId)
        {
            return studentCourse.GetStudentCourse(StudentId);
        }

        [HttpPost("Feesbalance")]
        public ResponsesWithData FeesBalance(string StudentId, string CourseId)
        {
            return studentCourse.GetBalanceAmount(StudentId, CourseId);
        }
    }
}
