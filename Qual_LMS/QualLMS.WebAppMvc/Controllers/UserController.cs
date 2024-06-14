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
                if (logger.IsLogged)
                {
                    var data = client.ExecutePostAPI<List<UserAllData>>("account/all?Id=" + logger.LoginDetails.OrganizationId.ToString());
                    return View(data);
                }
                else
                {
                    return RedirectToActionPermanent("Index", "Login");
                }
            }
        }

        public IActionResult Add(string Id)
        {
            ViewBag.Role = Request.Query["RId"];

            if (logger.IsLogged)
            {
                logger.ClearMessages();
                UserAllData Model = new UserAllData();
                //if (!string.IsNullOrEmpty(Id))
                //{
                //    Model = client.ExecutePostAPI<UserAllData>("account/get?Id=" + Id);
                //}
                return View(Model);
            }
            else
            {
                return RedirectToActionPermanent("Index", "Login");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add(UserAllData model)
        {
            ViewBag.Role = Request.Query["RId"];
            string json = JsonSerializer.Serialize(model);
            var res = client.ExecutePostAPI<ResultCommon>("account/registeraccount", json);

            //ResultCommon res = JsonSerializer.Deserialize<ResultCommon>(returnModel)!;
            logger.IsError = res.Error;
            logger.IsSuccess = !res.Error;

            if (logger.IsError)
            {
                string msg = res.Message;
                if (res.ApiError != null)
                {
                    if (res.ApiError.Errors != null)
                    {
                        foreach(var er in res.ApiError.Errors)
                        {
                            if (er.Value != null)
                            {
                                foreach (var val in er.Value)
                                {
                                    msg += "<br/>" + val;
                                }
                            }
                        }
                    }
                }
                logger.ErrorMessage = msg;
                return View(model);
            }
            else
            {
                logger.SuccessMessage = res.Message;
                return View(new UserAllData());
            }

        }
    }
}
