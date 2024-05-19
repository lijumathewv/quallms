using Microsoft.AspNetCore.Mvc.RazorPages;
using QualLMS.Domain.APIModels;

namespace QualLMS.WebApp.Pages
{
    public class OrganizationModel : PageModel
    {
        private readonly IConfiguration _configuration;
        public string? BaseURL { get; set; }

        public List<OrganizationData> Data { get; set; }

        public OrganizationModel(IConfiguration configuration)
        {
            _configuration = configuration;

            var appsettings = _configuration.GetSection("AppSettings");
            BaseURL = appsettings.GetSection("API").Value;

            Data = new List<OrganizationData>();
        }
        public void OnGet()
        {
        }
    }
}
