using Microsoft.AspNetCore.Mvc;
using QualLMS.Domain.APIModels;
using QualLMS.Domain.Models;
using QualLMS.WebAppMvc.Models;
using QualvationLibrary;
using System.Text.Json;

namespace QualLMS.WebAppMvc.Controllers
{
    public class UserController(CustomLogger logger, Client client) : Controller
    {
        public IActionResult Index()
        {
            Roles role = Roles.None;
            Enum.TryParse<Roles>(logger.LoginDetails.Role, out role);

            if (role != Roles.Admin && role != Roles.SuperAdmin)
            {
                return RedirectToAction("Unauthorized", "Home");
            }
            else
            {
                return View(new UserRegister());
            }
        }

        public IActionResult Add()
        {
            ViewBag.Role = Request.Query["RId"];
            return View(new UserRegister());
        }

        [HttpPost]
        public async Task<IActionResult> Add(UserRegister model)
        {
            ViewBag.Role = Request.Query["RId"];
            string json = JsonSerializer.Serialize(model);
            var returnModel = client.ExecutePostAPI<ResultCommon>("account/registeraccount", json);

            //ResultCommon res = JsonSerializer.Deserialize<ResultCommon>(returnModel)!;
            //logger.IsError = !res.Flag;
            //logger.IsSuccess = res.Flag;

            //if (logger.IsError)
            //{
            //    logger.ErrorMessage = res.Message;
            //}
            //else
            //{
            //    logger.SuccessMessage = res.Message;
            //}

            return View(new UserRegister());
        }
    }
}
