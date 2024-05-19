using Microsoft.AspNetCore.Mvc.RazorPages;
using QualLMS.Domain.APIModels;
using QualLMS.Domain.Models;
using QualLMS.WebApp.Data;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace QualLMS.WebApp.Pages
{
    public class AddOrganizationModel : PageModel
    {
        private readonly IConfiguration _configuration;
        public string? BaseURL { get; set; }

        public bool IsErrorMessage { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;

        public bool IsSuccessMessage { get; set; }
        public string SuccessMessage { get; set; } = string.Empty;


        public AddOrganizationModel(IConfiguration configuration)
        {
            _configuration = configuration;

            var appsettings = _configuration.GetSection("AppSettings");
            BaseURL = appsettings.GetSection("API").Value;

        }

        public void OnGet()
        {
            IsErrorMessage = false;
            IsSuccessMessage = false;
        }

        public async Task OnPostSubmitAsync(OrganizationData model)
        {
            Client client = new Client(BaseURL);
            string json = JsonSerializer.Serialize(model);
            var returnModel = await client.PostAPI("organization/add", json);

            ResultCommon res = JsonSerializer.Deserialize<ResultCommon>(returnModel)!;
            IsErrorMessage = !res.Flag;
            IsSuccessMessage = res.Flag;

            if (IsErrorMessage)
            {
                ErrorMessage = res.Message;
            }
            else
            {
                SuccessMessage = res.Message + " Please create an admin user.";
            }
        }
    }
}
