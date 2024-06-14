using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using QualLMS.Domain.APIModels;
using QualLMS.Domain.Models;
using QualLMS.WebAppMvc.Models;
using QualvationLibrary;
using System.Text.Json;

namespace QualLMS.WebAppMvc.Controllers
{
    public class OrganizationController(IConfiguration configuration, CustomLogger logger, ILogger<HomeController> logger1, Client client) : Controller
    {
        public IActionResult Index()
        {
            if (logger.IsLogged)
            {
                var data = client.ExecutePostAPI<List<Organization>>("Organization/all");
                return View(data);
            }
            else
            {
                return RedirectToActionPermanent("Index", "Login");
            }
        }

        public IActionResult Add()
        {
            ViewBag.IsError = false;
            ViewBag.IsSuccess = false;
            OrganizationData model = new OrganizationData();
            return View(model);
        }

        [HttpPost]
        public IActionResult Add(OrganizationData model)
        {
            string json = JsonSerializer.Serialize(model);

            var response = client.ExecutePostAPI<ResultCommon>("organization/add", json);

            //ResultCommon res = JsonSerializer.Deserialize<ResultCommon>(returnModel)!;
            //ViewBag.IsError = !res.Flag;
            //ViewBag.IsSuccess = res.Flag;

            //if (ViewBag.IsError)
            //{
            //    ViewBag.ErrorMessage = res.Message;
            //}
            //else
            //{
            //    ViewBag.SuccessMessage = res.Message + " Please create an admin user.";
            //}
            return View(model);
        }
    }
}
