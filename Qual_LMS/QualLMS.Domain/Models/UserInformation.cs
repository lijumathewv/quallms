using System.ComponentModel.DataAnnotations;

namespace QualLMS.Domain.Models
{
    public class UserInformation
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        [StringLength(450)]
        public string AppUserId { get; set; } = string.Empty;
        public virtual User AppUser { get; set; } = null!;

        [Required]
        public string Address { get; set; } = string.Empty;

        [Required]
        public string RecentEducation { get; set; } = string.Empty;

        [Required]
        public Guid CourseId { get; set; }
        public virtual Course Course { get; set; } = null!;

        public string AdmissionNumber { get; set; } = string.Empty;

    }
}
