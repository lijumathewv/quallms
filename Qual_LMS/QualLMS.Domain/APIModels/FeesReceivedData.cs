using QualLMS.Domain.Models;
using QualvationLibrary;
using System.ComponentModel.DataAnnotations;

namespace QualLMS.Domain.APIModels
{
    public class FeesReceivedData
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid StudentId { get; set; }
        public string StudentName { get; set; } = string.Empty;

        [Required]
        public Guid CourseId { get; set; }
        public string CourseName { get; set; } = string.Empty;

        [Required]
        public Guid FeesId { get; set; }
        public string FeesName { get; set; } = string.Empty;

        public string ReceiptNumber { get; set; } = string.Empty;

        public DateOnly ReceiptDate { get; set; }

        [AmountComparison("BalanceAmount")]
        public int ReceiptFees { get; set; }

        [Required]
        public int BalanceAmount { get; set; }

        public PaymentMode Mode { get; set; }

        public string ModeDetails { get; set; } = string.Empty;

        [Required]
        public Guid OrganizationId { get; set; }

    }
}
