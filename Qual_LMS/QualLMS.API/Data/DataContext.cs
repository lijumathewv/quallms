using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using QualLMS.Domain.Models;

namespace QualLMS.API.Data
{
    public class DataContext : IdentityDbContext<AppUser>
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Attendance> Attendance { get; set; }
        public DbSet<Organization> Organization { get; set; }
        public DbSet<UserInformation> UserInformation { get; set; }
    }
}
