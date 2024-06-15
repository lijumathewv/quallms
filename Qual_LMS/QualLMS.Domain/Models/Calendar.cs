using System.ComponentModel.DataAnnotations;

namespace QualLMS.Domain.Models
{
    public class Calendar
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;
        public virtual User Teacher { get; set; }

        [Required]
        public Guid CourseId { get; set; }
        public virtual Course Course { get; set; }

        [Required]
        public DateOnly Date { get; set; }

        [Required]
        public TimeOnly StartTime { get; set; }

        [Required]
        public TimeOnly EndTime { get; set; }
    }
}
