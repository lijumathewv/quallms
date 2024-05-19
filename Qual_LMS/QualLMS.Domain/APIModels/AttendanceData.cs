using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace QualLMS.Domain.APIModels
{
    public class AttendanceData
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [Required]
        [JsonPropertyName("appUserId")]
        public string AppId { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.DateTime)]
        [JsonPropertyName("attendanceDate")]
        public DateTime CurrentDate { get; set; }

        [DataType(DataType.DateTime)]
        [JsonPropertyName("checkIn")]
        public DateTime CheckIn { get; set; }

        [DataType(DataType.DateTime)]
        [JsonPropertyName("checkOut")]
        public DateTime CheckOut { get; set; }
    }
}
