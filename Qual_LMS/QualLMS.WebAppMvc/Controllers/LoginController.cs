using Microsoft.AspNetCore.Mvc;
using QualLMS.Domain.Models;
using QualLMS.WebAppMvc.Models;
using QualvationLibrary;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace QualLMS.WebAppMvc.Controllers
{
    public class LoginController(IConfiguration configuration, Client client, CustomLogger logger) : Controller
    {
        public IActionResult Index()
        {
            Login model = new Login();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Index(Login model)
        {
            try
            {
                var appsettings = configuration.GetSection("AppSettings");
                logger.BaseURL = appsettings.GetSection("API").Value!;

                string json = JsonSerializer.Serialize(model);
                var res = await client.PostSignInUpAPI("account/login", json);

                logger.IsError = res.Error;
                logger.ErrorMessage = res.Message;

                if (res.ReturnModel != null)
                {
                    var res1 = JsonSerializer.Deserialize<LoginProperties>(res.ReturnModel);

                    if (res1 != null)
                    {
                        logger.IsError = res1.Flag;
                        logger.ErrorMessage = res1.Message;
                        logger.LoginDetails = res1;
                    }
                }
                if (!logger.IsError)
                {
                    logger.IsLogged = true;

                    logger.ClearMessages();

                    return RedirectToActionPermanent("Index", "Home");
                }

                return View(new Login());
            }
            catch (Exception ex)
            {
                logger.GenerateException(ex);
                return View(new Login());
            }
        }
    }
}
