using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace QualLMS.Domain.APIModels
{
    public class OrganizationData
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [Required]
        [JsonPropertyName("fullname")]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [JsonPropertyName("emailid")]
        [DataType(DataType.EmailAddress)]
        public string EmailId { get; set; } = string.Empty;

        [Required]
        [JsonPropertyName("phonenumber")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required]
        [JsonPropertyName("address")]
        [DataType(DataType.Text)]
        public string Address { get; set; } = String.Empty;

        [Required]
        [JsonPropertyName("domainname")]
        [DataType(DataType.Text)]
        public string DomainName { get; set; } = String.Empty;

        [Required]
        [JsonPropertyName("geolocation")]
        [DataType(DataType.Text)]
        public string OfficeGeoLocation { get; set; } = String.Empty;
    }
}
