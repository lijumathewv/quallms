using System.Text.Json.Serialization;

namespace QualLMS.WebApp.Data
{
    public class ResultCommon
    {
        [JsonPropertyName("flag")]
        public bool Flag { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; } = string.Empty;
    }
}
