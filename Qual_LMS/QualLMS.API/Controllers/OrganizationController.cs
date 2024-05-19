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
        public async Task<IActionResult> AddOrganization(OrganizationData model)
        {
            var response = organization.Add(model);
            return Ok(response);
        }
        [HttpPost("all")]
        public async Task<IActionResult> AllOrganization()
        {
            var response = organization.Get();
            return Ok(response);
        }
    }
}
