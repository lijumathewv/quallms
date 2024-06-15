using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace QualLMS.Domain.Models
{
    public class User : IdentityUser
    { 
        public string? FullName { get; set; }

        public string? ParentName { get; set; }

        public string? ParentNumber { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? ModifiedAt { get; set; }

        [Required]
        public Guid OrganizationId { get; set; }
        public virtual Organization Organization { get; set; } = null!;

        public virtual ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();
    }
}
