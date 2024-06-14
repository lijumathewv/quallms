using System.ComponentModel.DataAnnotations;

namespace QualLMS.Domain.Models
{
    public class ParentInformation
    {
        [Key] 
        public Guid Id { get; set; }

        [Required]
        public string ParentName { get; set; } = string.Empty;

        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        public string EmailAddress { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; } = string.Empty;

        public virtual ICollection<User> AppUser { get; set; } = new List<User>();
    }
}
