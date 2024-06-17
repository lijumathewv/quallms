using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using QualLMS.Domain.APIModels;
using QualLMS.Domain.Contracts;
using QualLMS.Domain.Models;
using QualLMS.WebAppMvc.Models;
using QualvationLibrary;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace QualLMS.WebAppMvc.Controllers
{
    public class UserController(CustomLogger logger, Client client, IApplicationUserAccount repo, LoginProperties login) : Controller
    {
        public IActionResult Index()
        {
            if (login.Id == Guid.Empty)
            {
                login = JsonSerializer.Deserialize<LoginProperties>(HttpContext.Session.GetString("LoginDetails"));
            }

            Roles role = login.Role;

            if (role != Roles.Admin && role != Roles.SuperAdmin)
            {
                return RedirectToAction("Unauthorized", "Home");
            }
            else
            {
                var response = repo.AllUsers(login.OrganizationId);// client.ExecutePostAPI<List<UserAllData>>("account/all?Id=" + login.OrganizationId.ToString());
                return View(client.ParseResult<List<UserAllData>>(response.returnmodel));
            }
        }

        public IActionResult Add(string Id)
        {
            if (login.Id == Guid.Empty)
            {
                login = JsonSerializer.Deserialize<LoginProperties>(HttpContext.Session.GetString("LoginDetails"));
            }
            logger.ClearMessages();
            AddUserAllData Model = new AddUserAllData();

            Roles LoginRole = login.Role;
            Model.LoginRole = LoginRole;

            Model.orgSelect = new SelectList(new List<Organization>());
            if (LoginRole == Roles.SuperAdmin)
            {
                var response = repo.AllUsers(login.OrganizationId);//client.ExecutePostAPI<List<Organization>>("Organization/all");

                Model.orgSelect = new SelectList(client.ParseResult<List<Organization>>(response.returnmodel), "Id", "FullName");
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
            if (login.Id == Guid.Empty)
            {
                login = JsonSerializer.Deserialize<LoginProperties>(HttpContext.Session.GetString("LoginDetails"));
            }
            if (model.OrganizationId == Guid.Empty)
            {
                model.OrganizationId = login.OrganizationId;
            }

            //string json = JsonSerializer.Serialize(model);
            //var res = client.ExecutePostAPI<ResultCommon>("account/registeraccount", json);

            UserRegister Model = new UserRegister
            {
                Id = model.Id,
                FullName = model.FullName,
                ParentName = model.ParentName,
                ParentNumber = model.ParentNumber,
                OrganizationId = model.OrganizationId,
                RoleId = (Roles)model.RoleId,
                EmailId = model.EmailId,
                PhoneNumber = model.PhoneNumber,
                Password = model.Password,
                ConfirmPassword = model.ConfirmPassword,
            };

            var response = repo.CreateAccount(Model); //client.ExecutePostAPI<ResultCommon>("StudentCourse/add", JsonSerializer.Serialize(Model));

            if (!response.flag)
            {
                logger.IsError = true;
                logger.ErrorMessage = response.message;
            }
            //ResultCommon res = JsonSerializer.Deserialize<ResultCommon>(returnModel)!;
            logger.IsSuccess = response.flag;
            logger.SuccessMessage = response.message;

            if (logger.IsError)
            {
                //string msg;
                //if (res.ApiError != null)
                //{
                //    msg = res.ApiError.Title;
                //    if (res.ApiError.Errors != null)
                //    {
                //        foreach (var er in res.ApiError.Errors)
                //        {
                //            if (er.Value != null)
                //            {
                //                foreach (var val in er.Value)
                //                {
                //                    msg += "<br/>" + val;
                //                }
                //            }
                //        }
                //    }
                //    else
                //    {
                //        msg = res.Message;
                //    }
                //}
                //else
                //{
                //    msg = res.Message;
                //}
                //logger.ErrorMessage = msg;
                return View(model);
            }
            else
            {
                //logger.SuccessMessage = res.Message;
                return RedirectToActionPermanent("Index");
            }

        }
    }
}
