using QualvationLibrary;
using System.ComponentModel.DataAnnotations;

namespace QualLMS.Domain.Models
{
    public class ApplicationUser
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        public string EmailId { get; set; } = string.Empty;

        [Required]
        [Phone]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).+$", ErrorMessage = "Must meet password complexity requirements")]
        public string Password { get; set; } = string.Empty;

        public string? FullName { get; set; }

        public string? ParentName { get; set; }

        public string? ParentNumber { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? ModifiedAt { get; set; }

        public Roles Role { get; set; }

        [Required]
        public Guid OrganizationId { get; set; }
        public virtual Organization Organization { get; set; } = null!;
    }
}
