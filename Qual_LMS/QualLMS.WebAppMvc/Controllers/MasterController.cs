using Microsoft.AspNetCore.Mvc;
using QualLMS.Domain.APIModels;
using QualLMS.Domain.Models;
using QualvationLibrary;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace QualLMS.WebAppMvc.Controllers
{
    public class MasterController(Client client, CustomLogger logger) : Controller
    {
        [HttpGet("Course")]
        public IActionResult CourseList()
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

                var data = client.ExecutePostAPI<List<Course>>("Course/all?OrgId=" + logger.LoginDetails.OrganizationId);

                return View(data);
            }
            catch (Exception ex)
            {
                logger.GenerateException(ex);
                return View(new List<Course>());
            }
        }

        [HttpPost("AddCourse")]
        public IActionResult AddCourseList(IFormCollection form)
        {
            try
            {
                Guid CourseId = Guid.Empty;

                if (!string.IsNullOrEmpty(form["CourseId"]))
                {
                    CourseId = new Guid(form["CourseId"].ToString());
                }

                var model = new Course
                {
                    Id = CourseId,
                    OrganizationId = logger.LoginDetails.OrganizationId,
                    CourseFees = Convert.ToInt32(form["CourseFees"].ToString()),
                    CourseName = form["CourseName"].ToString()
                };

                var data = client.ExecutePostAPI<ResultCommon>("Course/add", JsonSerializer.Serialize(model));

                if (data.Error)
                {
                    throw new Exception("Error Occured:" + data.Message);
                }
                return RedirectToActionPermanent("CourseList");
            }
            catch (Exception ex)
            {
                logger.GenerateException(ex);
                TempData["IsError"] = true;
                TempData["IsSuccess"] = false;

                return RedirectToActionPermanent("CourseList");
            }
        }

        [HttpGet("Fees")]
        public IActionResult FeesList()
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

                var data = client.ExecutePostAPI<List<Fees>>("Fees/all?OrgId=" + logger.LoginDetails.OrganizationId);

                return View(data);
            }
            catch (Exception ex)
            {
                logger.GenerateException(ex);

                return View(new List<Fees>());
            }
        }

        [HttpPost("AddFees")]
        public IActionResult AddFeesList(IFormCollection form)
        {
            try
            {
                Guid FeesId = Guid.Empty;

                if (!string.IsNullOrEmpty(form["FeesId"]))
                {
                    FeesId = new Guid(form["FeesId"].ToString());
                }

                var model = new Fees
                {
                    Id = FeesId,
                    OrganizationId = logger.LoginDetails.OrganizationId,
                    FeesName = form["FeesName"].ToString()
                };

                var data = client.ExecutePostAPI<ResultCommon>("Fees/add", JsonSerializer.Serialize(model));

                if (data.Error)
                {
                    throw new Exception("Error Occured!" + data.Message);
                }
                return RedirectToActionPermanent("FeesList");
            }
            catch (Exception ex)
            {
                logger.GenerateException(ex);
                TempData["IsError"] = true;
                TempData["IsSuccess"] = false;

                return RedirectToActionPermanent("FeesList");
            }
        }
    }
}
