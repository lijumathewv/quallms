using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using QualLMS.Domain.Contracts;
using QualLMS.Domain.Models;

namespace QualLMS.Repository
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ApplicationUser>().HasIndex(u => u.EmailId).IsUnique();
        }

        public DbSet<ApplicationUser> ApplicationUser { get; set; }

        public DbSet<Attendance> Attendance { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<StudentCourse> StudentCourse { get; set; }
        public DbSet<Course> Course { get; set; }
        public DbSet<Fees> Fees { get; set; }
        public DbSet<FeesReceived> FeesReceived { get; set; }
        public DbSet<Calendar> Calendar { get; set; }
    }
}
