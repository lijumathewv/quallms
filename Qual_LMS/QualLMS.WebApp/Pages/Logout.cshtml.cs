using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace QualLMS.WebApp.Pages
{
    public class LogoutModel : PageModel
    {
        public void OnGet()
        {
            HttpContext.Session.Clear();
            Response.Redirect("/Login");
        }
    }
}
