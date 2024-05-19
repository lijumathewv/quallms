
using System.ComponentModel.DataAnnotations;

namespace QualLMS.Domain.Models
{
    public class Login
    {
        [Required]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        public string EmailId { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = String.Empty;
    }
}
