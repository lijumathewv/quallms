using Microsoft.AspNetCore.Mvc;
using QualLMS.Domain.Contracts;
using QualLMS.Domain.Models;
using QualLMS.WebAppMvc.Models;
using QualvationLibrary;
using Serilog;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace QualLMS.WebAppMvc.Controllers
{
    public class LoginController(IConfiguration configuration, Client client, CustomLogger logger, IApplicationUserAccount repo) : BaseController
    {
        public IActionResult Index()
        {
            Login model = new Login();
            return View(model);
        }

        [HttpPost]
        public IActionResult Index(Login model)
        {
            try
            {
                string json = JsonSerializer.Serialize(model);
                var res = repo.LoginAccount(model);

                logger.LoginRole = res.value.Role;
                logger.OrganizationId = res.value.OrganizationId;
                logger.LoginId = res.value.Id;

                SetSessionValue("LoginId", res.value.Id.ToString());
                SetSessionValue("LoginRole", res.value.Role.ToString());
                SetSessionValue("OrganizationId", res.value.OrganizationId.ToString());

                //HttpContext.Session.SetString("OrganizationId", res.value.OrganizationId.ToString());
                //HttpContext.Session.SetString("LoginId", res.value.Id.ToString());
                //HttpContext.Session.SetString("LoginDetails", JsonSerializer.Serialize(res.value));

                logger.IsError = res.value.IsError;
                logger.ErrorMessage = res.value.Message;

                if (!logger.IsError)
                {
                    logger.IsLogged = true;

                    logger.ClearMessages();

                    return RedirectToActionPermanent("Index", "Home");
                }
                else
                {
                    return View(model);
                }

            }
            catch (Exception ex)
            {
                logger.GenerateException(ex);
                return View(new Login());
            }
        }
    }
}
