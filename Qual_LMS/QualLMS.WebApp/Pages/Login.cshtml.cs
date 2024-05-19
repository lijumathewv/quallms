using Microsoft.AspNetCore.Mvc.RazorPages;
using QualLMS.Domain.Models;
using System.Text.Json;
using System.Text;
using System.Text.Json.Serialization;
using QualLMS.WebApp.Data;

namespace QualLMS.WebApp.Pages
{
    public class LoginModel : PageModel
    {
        private readonly IConfiguration _configuration;
        public string? BaseURL { get; set; }

        public bool IsErrorMessage { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;


        public LoginModel(IConfiguration configuration)
        {
            _configuration = configuration;

            var appsettings = _configuration.GetSection("AppSettings");
            BaseURL = appsettings.GetSection("API").Value;

        }

        public void OnGet()
        {
            IsErrorMessage = false;
        }

        public async Task OnPostSubmitAsync(Login model) 
        {
            Client client = new Client(BaseURL);
            string json = JsonSerializer.Serialize(model);
            var returnModel = await client.PostAPI("account/login", json);

            ResultLogin res = JsonSerializer.Deserialize<ResultLogin>(returnModel)!;
            IsErrorMessage = !res.Value!.Flag;
            if (IsErrorMessage)
            {
                ErrorMessage = res.Value.Message;
            }
            else
            {
                HttpContext.Session.SetString("LoginProperties", JsonSerializer.Serialize(res.Value));
                HttpContext.Session.SetInt32("IsLogged", 1);
                Response.Redirect("/Index");
            }
        }
    }

    public class ResultLogin
    {
        [JsonPropertyName("value")]
        public LoginProperties? Value { get; set; }
    }
}
