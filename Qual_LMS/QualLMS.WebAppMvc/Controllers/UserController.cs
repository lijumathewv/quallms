using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
                Roles role = logger.LoginDetails.Role;

                if (role != Roles.Admin && role != Roles.SuperAdmin)
                {
                    return RedirectToAction("Unauthorized", "Home");
                }
                else
                {
                    var data = client.ExecutePostAPI<List<UserAllData>>("account/all?Id=" + logger.LoginDetails.OrganizationId.ToString());
                    return View(data);
                }
            }
            catch (Exception ex)
            {
                logger.GenerateException(ex);
                logger.ErrorMessage = logger.GenerateDetailedException(ex);
                return View(new List<UserAllData>());
            }
        }

        public IActionResult Add(string Id)
        {
            logger.ClearMessages();
            AddUserAllData Model = new AddUserAllData();

            Roles LoginRole = logger.LoginDetails.Role;
            Model.LoginRole = LoginRole;

            Model.orgSelect = new SelectList(new List<Organization>());
            if (LoginRole == Roles.SuperAdmin)
            {
                var orgs = client.ExecutePostAPI<List<Organization>>("Organization/all");

                Model.orgSelect = new SelectList(orgs, "Id", "FullName");
            }

            Roles role = Roles.None;

            if (Model.Id != Guid.Empty)
            {
                Enum.TryParse<Roles>(Model.Role, out role);
                Model.role = role;
                Model.RoleId = (int)role;
            }
            else
            {
                Model.RoleId = Convert.ToInt32(Request.Query["RId"]);
                Model.role = (Roles)Model.RoleId;
            }

            return View(Model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddUserAllData model)
        {
            try
            {
                if (model.OrganizationId == Guid.Empty)
                {
                    model.OrganizationId = logger.LoginDetails.OrganizationId;
                }

                string json = JsonSerializer.Serialize(model);
                var res = client.ExecutePostAPI<ResultCommon>("account/registeraccount", json);

                //ResultCommon res = JsonSerializer.Deserialize<ResultCommon>(returnModel)!;
                logger.IsError = res.Error;
                logger.IsSuccess = !res.Error;

                if (logger.IsError)
                {
                    string msg;
                    if (res.ApiError != null)
                    {
                        msg = res.ApiError.Title;
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
                        else
                        {
                            msg = res.Message;
                        }
                    }
                    else
                    {
                        msg = res.Message;
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
                Roles role = Roles.None;

                if (model.Id != Guid.Empty)
                {
                    Enum.TryParse<Roles>(model.Role, out role);
                    model.role = role;
                    model.RoleId = (int)role;
                }
                else
                {
                    model.RoleId = Convert.ToInt32(Request.Query["RId"]);
                    model.role = (Roles)model.RoleId;
                }
                logger.GenerateException(ex);
                logger.ErrorMessage = logger.GenerateDetailedException(ex);
                return View(model);
            }

        }
    }
}
