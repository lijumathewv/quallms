
using QualLMS.Domain.Models;
using System.Text.Json.Serialization;

namespace QualLMS.WebAppMvc.Models
{
    public class Common
    {
        public string BaseURL { get; set; } = string.Empty;

        public bool IsLogged { get; set; } = false;

        public LoginProperties? loginProperties { get; set; }

        public bool IsError { get; set; } = false;

        public bool IsSuccess { get; set; } = false;

        public string ErrorMessage { get; set; } = string.Empty;

        public string SuccessMessage { get; set; } = string.Empty;
    }

    public class ResultCommon
    {
        [JsonPropertyName("flag")]
        public bool Flag { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; } = string.Empty;
    }
}
