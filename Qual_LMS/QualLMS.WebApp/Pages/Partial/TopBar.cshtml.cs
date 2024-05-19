using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using QualLMS.Domain.Models;
using System.Text.Json;

namespace QualLMS.WebApp.Pages.Partial
{
    public class TopBarModel : PageModel
    {
        public LoginProperties? LoginProperties { get; set; }

        public TopBarModel(IHttpContextAccessor _httpContext)
        {
            LoginProperties = new LoginProperties();
        }

        public void OnGet()
        {
        }
    }
}
