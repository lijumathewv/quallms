using Microsoft.AspNetCore.Mvc;
using QualLMS.Domain.APIModels;
using QualLMS.Domain.Contracts;

namespace QualLMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentCourseController(IStudentCourse repository) : ControllerBase
    {
        [HttpPost("add")]
        public IActionResult Add(StudentCourseData model)
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
        public IActionResult GetAll()
        {
            var response = repository.Get();
            return Ok(response);
        }

        [HttpPost("getsingle")]
        public IActionResult GetSingle(string Id)
        {
            var response = repository.Get(Id);
            return Ok(response);
        }

        [HttpPost("studentcourse")]
        public IActionResult GetStudentCourse(string StudentId)
        {
            var response = repository.GetStudentCourse(StudentId);
            return Ok(response);
        }

        [HttpPost("balance")]
        public IActionResult BalanceAmount(string StudentId, string CourseId)
        {
            var response = repository.GetBalanceAmount(StudentId, CourseId);
            return Ok(response);
        }
    }
}
