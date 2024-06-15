namespace QualLMS.Domain.APIModels
{
    public class UserAllData
    {
        public Guid Id { get; set; }

        public Guid OrganizationId { get; set; }

        public string FullName { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        public string EmailId { get; set; } = string.Empty;

        public string ParentName { get; set; } = string.Empty;

        public string ParentNumber { get; set; } = string.Empty;

        public string Role { get; set; } = string.Empty;

        public int RoleId { get; set; }

        public string Password { get; set; } = string.Empty;

        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
