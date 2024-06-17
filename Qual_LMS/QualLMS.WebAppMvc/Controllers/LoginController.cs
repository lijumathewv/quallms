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
    public class LoginController(IConfiguration configuration, Client client, CustomLogger logger, IApplicationUserAccount repo, LoginProperties login) : Controller
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

                HttpContext.Session.SetString("LoginDetails", JsonSerializer.Serialize(res.value));

                login = res.value;

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
