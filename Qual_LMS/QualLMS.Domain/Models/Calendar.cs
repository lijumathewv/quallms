using System.ComponentModel.DataAnnotations;

namespace QualLMS.Domain.Models
{
    public class Calendar
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid TeacherId { get; set; }
        public virtual ApplicationUser? Teacher { get; set; }

        [Required]
        public Guid CourseId { get; set; }
        public virtual Course? Course { get; set; }

        [Required]
        public DateOnly Date { get; set; }

        [Required]
        public TimeOnly StartTime { get; set; }

        [Required]
        public TimeOnly EndTime { get; set; }

        [Required]
        public Guid OrganizationId { get; set; }
        public virtual Organization? Organization { get; set; }
    }
}
