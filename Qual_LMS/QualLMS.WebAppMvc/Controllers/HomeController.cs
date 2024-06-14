using Microsoft.AspNetCore.Mvc;
using QualLMS.WebAppMvc.Models;
using QualvationLibrary;
using System.Diagnostics;

namespace QualLMS.WebAppMvc.Controllers
{
    public class HomeController(CustomLogger logger, ILogger<HomeController> logger1) : Controller
    {
        public IActionResult Index()
        {
            if (logger.IsLogged)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index","Login");
            }
        }

        public IActionResult Logout()
        {
            logger.IsLogged = false;
            logger.LoginDetails = null!;
            return RedirectToAction("Index", "Login");
        }

        public IActionResult Unauthorized()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
