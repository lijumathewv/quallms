using Microsoft.AspNetCore.Mvc;
using QualLMS.Domain.APIModels;
using QualLMS.Domain.Contracts;
using QualLMS.Domain.Models;
using QualvationLibrary;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace QualLMS.WebAppMvc.Controllers
{
    public class FeesController(Client client, CustomLogger logger, IFees repo, LoginProperties login) : Controller
    {
        [HttpGet("Fees")]
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

            var response = repo.GetAll(login.OrganizationId.ToString());//client.ExecutePostAPI<List<Fees>>("Fees/all?OrgId=" + login.OrganizationId);
            var data = client.ParseResult<List<Fees>>(response.returnmodel); 

            return View(data);
        }

        [HttpPost("AddFees")]
        public IActionResult AddFeesList(IFormCollection form)
        {
            if (login.Id == Guid.Empty)
            {
                login = JsonSerializer.Deserialize<LoginProperties>(HttpContext.Session.GetString("LoginDetails"));
            }

            Guid FeesId = Guid.Empty;

            if (!string.IsNullOrEmpty(form["FeesId"]))
            {
                FeesId = new Guid(form["FeesId"].ToString());
            }

            var model = new Fees
            {
                Id = FeesId,
                OrganizationId = login.OrganizationId,
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
    }
}
