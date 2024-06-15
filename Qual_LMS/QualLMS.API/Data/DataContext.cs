using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using QualLMS.Domain.Models;

namespace QualLMS.API.Data
{
    public class DataContext : IdentityDbContext<User>
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Attendance> Attendance { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<StudentCourse> StudentCourse { get; set; }
        public DbSet<Course> Course { get; set; }
        public DbSet<Fees> Fees { get; set; }
        public DbSet<FeesReceived> FeesReceived { get; set; }
        public DbSet<Calendar> Calendar { get; set; }
    }
}
