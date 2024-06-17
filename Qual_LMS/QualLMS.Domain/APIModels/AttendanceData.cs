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
        public Guid AppId { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [JsonPropertyName("attendanceDate")]
        public DateOnly? CurrentDate { get; set; }

        [DataType(DataType.DateTime)]
        [JsonPropertyName("checkIn")]
        public DateTime? CheckIn { get; set; }

        [DataType(DataType.DateTime)]
        [JsonPropertyName("checkOut")]
        public DateTime? CheckOut { get; set; }

        public string Role { get; set; } = string.Empty;

        public string FullName { get; set; } = string.Empty;
    }

    public class ViewAttendanceData
    {
        public List<AttendanceData> Data { get; set; }

        public bool IsCheckedIn { get; set; }

        public bool IsCheckedOut { get; set; }
    }
}
