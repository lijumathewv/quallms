using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using QualLMS.Domain.APIModels;
using QualLMS.Domain.Models;
using QualLMS.WebApp.Data;
using System.Text.Json;

namespace QualLMS.WebApp.Pages
{
    public class AttendanceModel : PageModel
    {
        private readonly IConfiguration _configuration;
        public string? BaseURL { get; set; }

        public LoginProperties? loginProperties { get; set; }

        public List<AttendanceData> Data { get; set; }

        public AttendanceModel(IConfiguration configuration)
        {
            _configuration = configuration;

            var appsettings = _configuration.GetSection("AppSettings");
            BaseURL = appsettings.GetSection("API").Value;

            Data = new List<AttendanceData>();
        }

        public void OnGet()
        {
        }

        public class APIReturnData
        {
            public bool flag { get; set; }
            public List<AttendanceData>? returnData { get; set; }
            public string? message { get; set; }
        }
    }
}
