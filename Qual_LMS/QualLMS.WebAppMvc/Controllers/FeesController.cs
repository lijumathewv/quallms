using Microsoft.AspNetCore.Mvc;
using QualLMS.Domain.APIModels;
using QualLMS.Domain.Contracts;
using QualLMS.Domain.Models;
using QualvationLibrary;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace QualLMS.WebAppMvc.Controllers
{
    public class FeesController(Client client, CustomLogger logger, IFees repo) : BaseController
    {
        [HttpGet("Fees")]
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

                var response = repo.GetAll(GetSessionValue("OrganizationId"));//client.ExecutePostAPI<List<Fees>>("Fees/all?OrgId=" + new Guid(GetSessionValue("OrganizationId")));
                var data = client.ParseResult<List<Fees>>(response.returnmodel);

                return View(data);
            }
            catch (Exception ex)
            {
                logger.GenerateException(ex);
                return StatusCode(500, "Internal server error");
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
                    OrganizationId = new Guid(GetSessionValue("OrganizationId")),
                    FeesName = form["FeesName"].ToString()
                };

                var response = repo.AddOrUpdate(model); //client.ExecutePostAPI<ResultCommon>("Fees/add", JsonSerializer.Serialize(model));

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
    }
}
