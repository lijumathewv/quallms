﻿using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace QualLMS.Domain.Models
{
    public class AppUser : IdentityUser
    { 
        public string? FullName { get; set; }

        public string? ParentName { get; set; }

        public string? ParentNumber { get; set; }

        [Required]
        public Guid OrganizationId { get; set; }
        public virtual Organization Organization { get; set; } = null!;

        public virtual ICollection<UserInformation> UserInformations { get; set; } = new List<UserInformation>();
        public virtual ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();
    }
}
