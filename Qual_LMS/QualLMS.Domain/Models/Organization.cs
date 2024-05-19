using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

namespace QualLMS.Domain.Models
{
    public class Organization
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        public string EmailId { get; set; } = string.Empty;

        [Required]
        [Phone]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Text)]
        public string Address { get; set; } = String.Empty;

        [Required]
        [DataType(DataType.Text)]
        public string DomainName { get; set; } = String.Empty;

        [Required]
        [DataType(DataType.Text)]
        public string OfficeGeoLocation { get; set; } = String.Empty;

        public virtual ICollection<AppUser> Users { get; set; } = new List<AppUser>();

        public virtual ICollection<Course> Courses { get; set; } = new List<Course>();

        public virtual ICollection<Fees> Fees { get; set; } = new List<Fees>();
    }
}
