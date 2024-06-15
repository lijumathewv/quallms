﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QualLMS.Domain.Models
{
    public class Attendance
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        [StringLength(450)]
        public string AppUserId { get; set; } = string.Empty;
        public virtual User AppUser { get; set; } = null!;

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