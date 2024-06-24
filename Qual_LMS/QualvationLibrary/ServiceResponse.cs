using Microsoft.Extensions.Caching.Memory;
using System.Text.Json.Serialization;

namespace QualvationLibrary
{
    public class ServiceResponse
    {
        public record class GeneralResponses(bool flag, string message);
        public record class LoginResponses(LoginProperties value);
        public record class ResponsesWithData(bool flag, string returnmodel, string message);
    }

    public class LoginProperties
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("flag")]
        public bool IsError { get; set; }

        [JsonPropertyName("emailid")]
        public string EmailId { get; set; } = string.Empty;

        [JsonPropertyName("token")]
        public string Token { get; set; } = string.Empty;
        [JsonPropertyName("refreshtoken")]
        public string RefreshToken { get; set; } = string.Empty;

        [JsonPropertyName("role")]
        public Roles Role { get; set; }

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

    //public class CacheService(IMemoryCache memoryCache)
    //{

    //    public void SetData(string key, object value)
    //    {
    //        memoryCache.Set(key, value, new MemoryCacheEntryOptions
    //        {
    //            SlidingExpiration = TimeSpan.FromHours(3)
    //        });
    //    }

    //    public object GetData(string key)
    //    {
    //        memoryCache.TryGetValue(key, out var value);
    //        return value;
    //    }
    //}

}
