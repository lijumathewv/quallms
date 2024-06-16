using QualLMS.Domain.Models;
using QualvationLibrary;
using System.ComponentModel.DataAnnotations;

namespace QualLMS.Domain.APIModels
{
    public class CalendarData
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        [StringLength(450)]
        public string TeacherId { get; set; } = string.Empty;

        public string TeacherName { get; set; } = string.Empty;

        [Required]
        public Guid CourseId { get; set; }

        public string CourseName { get; set; } = string.Empty;

        [Required]
        public DateOnly Date { get; set; }

        [TimeComparison("EndTime")]
        public TimeOnly StartTime { get; set; }

        [Required]
        public TimeOnly EndTime { get; set; }

        [Required]
        public Guid OrganizationId { get; set; }
    }
}
