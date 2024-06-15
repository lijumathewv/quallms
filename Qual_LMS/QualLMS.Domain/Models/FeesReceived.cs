using System.ComponentModel.DataAnnotations;

namespace QualLMS.Domain.Models
{
    public class FeesReceived
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;
        public virtual User User { get; set; } = null!;

        [Required]
        public Guid CourseId { get; set; }
        public virtual Course Course { get; set; } = null!;

        [Required]
        public Guid FeesId { get; set; }
        public virtual Fees Fees { get; set; } = null!;

        public string ReceiptNumber { get; set; } = string.Empty;

        public DateOnly ReceiptDate { get; set; }

        public int ReceiptFees { get; set; }

        public PaymentMode Mode { get; set; }

        public string ModeDetails { get; set; } = string.Empty;
    }

    public enum PaymentMode
    {
        None,
        Cash,
        Cheque,
        Netbanking
    }
}
