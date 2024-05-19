using Microsoft.AspNetCore.Mvc.RazorPages;
using QualLMS.Domain.Models;

namespace QualLMS.WebApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public Roles LoggedRole { get; set; }

        public void OnGet()
        {
            if (HttpContext.Session.GetInt32("IsLogged") == null)
            {
                Response.Redirect("/login", true);
            }
        }
    }
}
