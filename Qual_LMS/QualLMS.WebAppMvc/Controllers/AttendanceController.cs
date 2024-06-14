using Microsoft.AspNetCore.Mvc;

namespace QualLMS.WebAppMvc.Controllers
{
    public class AttendanceController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
