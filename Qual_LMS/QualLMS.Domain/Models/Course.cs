using System.ComponentModel.DataAnnotations;

namespace QualLMS.Domain.Models
{
    public class Course
    {
        [Required]
        public Guid Id { get; set; }

        [Required] 
        public string CourseName { get; set; } = string.Empty;

        [Required]
        public Guid OrganizationId { get; set; }
        public virtual Organization Organization { get; set; } = null!;

        public virtual ICollection<UserInformation> UserInformations { get; set; } = new List<UserInformation>();
    }
}
