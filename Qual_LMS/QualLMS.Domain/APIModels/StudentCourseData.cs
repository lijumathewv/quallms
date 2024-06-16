using QualLMS.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace QualLMS.Domain.APIModels
{
    public class StudentCourseData
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public Guid StudentId { get; set; }

        public string StudentName { get; set; } = string.Empty;

        [Required]
        public Guid CourseId { get; set; }

        public string CourseName { get; set; } = string.Empty;

        [Required]
        public string RecentEducation { get; set; } = string.Empty;

        public string AdmissionNumber { get; set; } = string.Empty;

        public int CourseFees { get; set; }

        [Required]
        public Guid OrganizationId { get; set; }
    }
}
