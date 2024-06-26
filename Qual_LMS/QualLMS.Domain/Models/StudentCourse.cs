﻿using System.ComponentModel.DataAnnotations;

namespace QualLMS.Domain.Models
{
    public class StudentCourse
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public Guid StudentId { get; set; }
        public virtual ApplicationUser? Student { get; set; }

        [Required]
        public Guid CourseId { get; set; }
        public virtual Course? Course { get; set; }

        [Required]
        public string RecentEducation { get; set; } = string.Empty;

        public string AdmissionNumber { get; set; } = string.Empty;

        public int CourseFees { get; set; }

        [Required]
        public Guid OrganizationId { get; set; }
        public virtual Organization? Organization { get; set; }

        public bool Completed { get; set; }
    }
}
