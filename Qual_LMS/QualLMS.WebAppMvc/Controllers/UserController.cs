using Microsoft.AspNetCore.Mvc;
using QualLMS.Domain.APIModels;
using QualLMS.Domain.Models;
using QualLMS.WebAppMvc.Models;
using QualvationLibrary;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace QualLMS.WebAppMvc.Controllers
{
    public class UserController(CustomLogger logger, Client client) : Controller
    {
        public IActionResult Index()
        {
            try
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
            catch (Exception ex)
            {
                logger.GenerateException(ex);
                return View(new List<UserAllData>());
            }
        }

        public IActionResult Add(string Id)
        {
            try
            {
                ViewBag.Role = Request.Query["RId"];

                if (logger.IsLogged)
                {
                    logger.ClearMessages();
                    UserAllData Model = new UserAllData();

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
                return View(new UserAllData());
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add(UserAllData model)
        {
            try
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
                            foreach (var er in res.ApiError.Errors)
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
                    throw new Exception(msg);
                }
                else
                {
                    logger.SuccessMessage = res.Message;
                    return RedirectToActionPermanent("Index");
                }
            }
            catch (Exception ex)
            {
                logger.GenerateException(ex);
                return View(model);
            }

        }
    }
}
