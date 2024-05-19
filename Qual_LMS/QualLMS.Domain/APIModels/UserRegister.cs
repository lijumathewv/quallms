using System.ComponentModel.DataAnnotations;

namespace QualLMS.Domain.APIModels
{
    public class UserRegister
    {
        public string Id { get; set; } = string.Empty;

        [Required]
        public string FullName { get; set; } = string.Empty;

        public string? ParentName { get; set; }

        public string? ParentNumber { get; set; }

        [Required]
        public Guid OrganizationId { get; set; }

        [Required]
        public int RoleId { get; set; }

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
        public string Password { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
