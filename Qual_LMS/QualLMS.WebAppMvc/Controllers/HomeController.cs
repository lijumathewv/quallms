using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using QualLMS.WebAppMvc.Data;
using QualLMS.WebAppMvc.Models;
using QualvationLibrary;
using System.Diagnostics;

namespace QualLMS.WebAppMvc.Controllers
{
    public class HomeController(CustomLogger logger, ILogger<HomeController> logger1) : BaseController
    {
        public IActionResult Index()
        {
            if (logger.IsLogged)
            {
                logger.ClearMessages();
                
                return View();
            }
            else
            {
                return RedirectToAction("Index","Login");
            }
        }

        public IActionResult Logout()
        {
            logger.ClearMessages();
            logger.IsLogged = false;
            return RedirectToAction("Index", "Login");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            ErrorViewModel ev = new ErrorViewModel();
            ev.RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;

            try
            {
                var execeptionHandlerPathFeture = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

                if (execeptionHandlerPathFeture != null)
                {
                    if (execeptionHandlerPathFeture.Error != null)
                    {
                        ev.ErrorMessage = execeptionHandlerPathFeture.Error.Message;
                        ev.Source = execeptionHandlerPathFeture.Error.Source;
                        ev.StackTrace = execeptionHandlerPathFeture.Error.StackTrace;
                        ev.InnerException = execeptionHandlerPathFeture.Error.InnerException;
                    }
                    ev.ErrorPath = execeptionHandlerPathFeture.Path;
                }
            }
            catch (Exception ex)
            {
                ev.ErrorMessage = ex.Message;
                ev.Source = ex.Source;
                ev.StackTrace = ex.StackTrace;
                ev.InnerException = ex.InnerException;
            }
            return View(ev);
        }
    }
}
