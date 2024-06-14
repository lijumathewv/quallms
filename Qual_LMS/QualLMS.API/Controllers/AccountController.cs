using Microsoft.AspNetCore.Mvc;
using QualLMS.Domain.APIModels;
using QualLMS.Domain.Contracts;
using QualLMS.Domain.Models;
using QualvationLibrary;

namespace QualLMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController(IUserAccount userAccount) : ControllerBase
    {
        [HttpPost("registeraccount")]
        public async Task<IActionResult> RegisterMe(UserRegister registerModel)
        {
            var response = await userAccount.CreateAccount(registerModel);
            return Ok(response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> ValidateLogin(Login login)
        {
            var response = await userAccount.LoginAccount(login);
            return Ok(response);
        }

        [HttpPost("all")]
        public async Task<IActionResult> AllUsersFromOrgnization(Guid Id)
        {
            var response = await userAccount.AllUsers(Id);
            return Ok(response);
        }

        [HttpPost("get")]
        public async Task<IActionResult> GetUser(string Id)
        {
            var response = await userAccount.GetUser(Id);
            return Ok(response);
        }
    }
}
