using Microsoft.AspNetCore.Mvc;
using QualLMS.Domain.APIModels;
using QualLMS.Domain.Contracts;
using QualLMS.Domain.Models;
using QualvationLibrary;
using System.Security.Cryptography;

namespace QualLMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController(IApplicationUserAccount userAccount) : ControllerBase
    {
        [HttpPost("registeraccount")]
        public IActionResult RegisterMe(UserRegister registerModel)
        {
            var response = userAccount.CreateAccount(registerModel);
            return Ok(response);
        }

        [HttpPost("login")]
        public IActionResult ValidateLogin(Login login)
        {
            var response = userAccount.LoginAccount(login);
            return Ok(response);
        }

        [HttpPost("all")]
        public IActionResult AllUsersFromOrgnization(Guid Id)
        {
            var response = userAccount.AllUsers(Id);
            return Ok(response);
        }

        [HttpPost("get")]
        public IActionResult GetUser(string Id)
        {
            var response = userAccount.GetUser(Id);
            return Ok(response);
        }

        [HttpPost("allteachers")]
        public IActionResult AllTeachers(string OrgId)
        {
            var response = userAccount.GetTeachers(OrgId);
            return Ok(response);
        }

        [HttpPost("allstudents")]
        public IActionResult AllStudents(string OrgId)
        {
            var response = userAccount.GetStudents(OrgId);
            return Ok(response);
        }
    }
}
