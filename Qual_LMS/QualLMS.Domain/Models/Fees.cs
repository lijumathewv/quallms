using System.ComponentModel.DataAnnotations;

namespace QualLMS.Domain.Models
{
    public class Fees
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string FeesName { get; set; } = string.Empty;

        [Required]
        public Guid OrganizationId { get; set; }
        public virtual Organization? Organization { get; set; }
    }
}
