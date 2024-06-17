using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using QualLMS.Domain.APIModels;
using QualLMS.Domain.Contracts;
using QualLMS.Domain.Models;
using QualLMS.WebAppMvc.Models;
using QualvationLibrary;
using System.Text.Json;

namespace QualLMS.WebAppMvc.Controllers
{
    public class OrganizationController(IConfiguration configuration, CustomLogger logger, ILogger<HomeController> logger1, IOrganization repo, Client client) : BaseController
    {
        public IActionResult Index()
        {
            try
            {
                var response = repo.Get();// client.ExecutePostAPI<List<Organization>>("Organization/all");
                if (!response.flag)
                {
                    logger.IsError = true;
                    logger.ErrorMessage = response.message;
                }

                var data = client.ParseResult<List<Organization>>(response.returnmodel);

                return View(data);
            }
            catch (Exception ex)
            {

                logger.GenerateException(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        public IActionResult Add(string Id)
        {
            try
            {
                Organization Model = new Organization();
                if (!string.IsNullOrEmpty(Id))
                {
                    var response = repo.Get(Id);//client.ExecutePostAPI<Organization>("Organization/get?Id=" + Id);
                    Model = client.ParseResult<Organization>(response.returnmodel);
                }

                return View(Model);
            }
            catch (Exception ex)
            {
                logger.GenerateException(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public IActionResult Add(Organization model)
        {
            try
            {
                //string json = JsonSerializer.Serialize(model);
                var res = repo.AddOrUpdate(model);//client.ExecutePostAPI<ResultCommon>("organization/add", json);

                if (res.flag)
                {
                    logger.IsSuccess = res.flag;
                    logger.SuccessMessage = res.message;
                }
                else
                {
                    logger.IsError = !res.flag;
                    logger.ErrorMessage = res.message;
                }
                return View(model);
            }
            catch (Exception ex)
            {
                logger.GenerateException(ex);
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
