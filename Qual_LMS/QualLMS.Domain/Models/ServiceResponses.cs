using System.Text.Json.Serialization;

namespace QualLMS.Domain.Models
{
    public class ServiceResponses
    {
        public record class GeneralResponses(bool flag, string message);
        public record class LoginResponses(LoginProperties value);
        public record class ResponsesWithData(bool flag, object returnData, string message);
    }

    public class LoginProperties
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("flag")]
        public bool Flag { get; set; }

        [JsonPropertyName("emailid")]
        public string EmailId { get; set; } = string.Empty;

        [JsonPropertyName("token")]
        public string Token { get; set; } = string.Empty;

        [JsonPropertyName("role")]
        public int Role { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; } = string.Empty;

        [JsonPropertyName("fullname")]
        public string FullName { get; set; } = string.Empty;

        [JsonPropertyName("parentname")]
        public string? ParentName { get; set; } = string.Empty;

        [JsonPropertyName("parentnumber")]
        public string? ParentNumber { get; set; } = string.Empty;

        [JsonPropertyName("organizationId")]
        public Guid OrganizationId { get; set; }
    }
}
