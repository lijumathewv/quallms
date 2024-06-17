using Microsoft.AspNetCore.Mvc;
using QualLMS.Domain.APIModels;
using QualLMS.Domain.Contracts;

namespace QualLMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeesReceivedController(IFeesReceived repository) : ControllerBase
    {
        [HttpPost("add")]
        public IActionResult Add(FeesReceivedData model)
        {
            var response = repository.AddOrUpdate(model);
            return Ok(response);
        }

        [HttpPost("delete")]
        public IActionResult Delete(string Id)
        {
            var response = repository.Delete(Id);
            return Ok(response);
        }

        [HttpPost("all")]
        public IActionResult GetAll(string OrgId)
        {
            var response = repository.GetAll(OrgId);
            return Ok(response);
        }

        [HttpPost("getsingle")]
        public IActionResult GetSingle(string Id)
        {
            var response = repository.Get(Id);
            return Ok(response);
        }
    }
}
