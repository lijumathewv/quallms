using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QualLMS.Domain.APIModels;
using QualLMS.Domain.Contracts;
using QualLMS.Domain.Models;

namespace QualLMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendanceController(IAttendance attendance) : ControllerBase
    {
        [HttpPost("checkin")]
        public async Task<IActionResult> CheckInMe(AttendanceData model)
        {
            var response = attendance.CheckIn(model);
            return Ok(response);
        }

        [HttpPost("checkout")]
        public async Task<IActionResult> CheckOutMe(AttendanceData model)
        {
            var response = attendance.CheckOut(model);
            return Ok(response);
        }

        [HttpPost("list-all-attendance")]
        public async Task<IActionResult> ListAllMyAttendance(string Id)
        {
            var response = attendance.GetMyAttendance(new Guid(Id));
            return Ok(response);
        }

        [HttpPost("todays-attendance")]
        public async Task<IActionResult> MyAttendanceForToday(string Id)
        {
            var response = attendance.GetAttendanceForToday(new Guid(Id));
            return Ok(response);
        }
    }
}
