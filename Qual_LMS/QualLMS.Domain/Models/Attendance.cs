using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QualLMS.Domain.Models
{
    public class Attendance
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public Guid UserId { get; set; }
        public virtual ApplicationUser? ApplicationUser { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateOnly? AttendanceDate { get; set; }

        [Required]
        [DataType(DataType.Time)]
        public DateTime? CheckIn { get; set; }

        [DataType(DataType.Time)]
        public DateTime? CheckOut { get; set; }
    }
}
