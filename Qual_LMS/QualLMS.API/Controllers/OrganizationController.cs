using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QualLMS.Domain.APIModels;
using QualLMS.Domain.Contracts;
using QualLMS.Domain.Models;

namespace QualLMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrganizationController(IOrganization organization) : ControllerBase
    {
        [HttpPost("add")]
        public IActionResult AddOrganization(Organization model)
        {
            var response = organization.AddOrUpdate(model);
            return Ok(response);
        }
        [HttpPost("all")]
        public IActionResult AllOrganization()
        {
            var response = organization.Get();
            return Ok(response);
        }
        [HttpPost("get")]
        public IActionResult GetOrganization(string Id)
        {
            var response = organization.Get(Id);
            return Ok(response);
        }
    }
}
