using System.ComponentModel.DataAnnotations;

namespace QualLMS.Domain.Models
{
    public class FeesReceived
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid StudentId { get; set; }
        public virtual ApplicationUser? Student { get; set; }

        [Required]
        public Guid CourseId { get; set; }
        public virtual Course? Course { get; set; }

        [Required]
        public Guid FeesId { get; set; }
        public virtual Fees? Fees { get; set; }

        public string ReceiptNumber { get; set; } = string.Empty;

        public DateOnly ReceiptDate { get; set; }

        public int ReceiptFees { get; set; }

        public PaymentMode Mode { get; set; }

        public string ModeDetails { get; set; } = string.Empty;

        [Required]
        public Guid OrganizationId { get; set; }
        public virtual Organization? Organization { get; set; }
    }

    public enum PaymentMode
    {
        None,
        Cash,
        Cheque,
        Netbanking
    }
}
