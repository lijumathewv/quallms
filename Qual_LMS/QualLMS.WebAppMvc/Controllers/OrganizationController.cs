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
            try
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
            catch (Exception ex)
            {
                logger.GenerateException(ex);
                return View(new List<Organization>());
            }
        }

        public IActionResult Add(string Id)
        {
            try
            {
                if (logger.IsLogged)
                {
                    Organization Model = new Organization();
                    if (!string.IsNullOrEmpty(Id))
                    {
                        Model = client.ExecutePostAPI<Organization>("Organization/get?Id=" + Id);
                    }
                    return View(Model);
                }
                else
                {
                    return RedirectToActionPermanent("Index", "Login");
                }
            }
            catch (Exception ex)
            {
                logger.GenerateException(ex);
                return View(new Organization());
            }
        }

        [HttpPost]
        public IActionResult Add(Organization model)
        {
            try
            {
                string json = JsonSerializer.Serialize(model);

                var res = client.ExecutePostAPI<ResultCommon>("organization/add", json);

                logger.IsError = res.Error;
                logger.IsSuccess = !res.Error;

                if (res.Error)
                {
                    throw new Exception(res.Message);
                }
                logger.SuccessMessage = res.Message;
                return View(model);
            }
            catch (Exception ex)
            {
                logger.GenerateException(ex);
                return View(model);
            }
        }
    }
}
