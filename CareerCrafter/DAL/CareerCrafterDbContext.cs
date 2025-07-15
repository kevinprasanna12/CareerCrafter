using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
    public class CareerCrafterDbContext : DbContext
    {
        public CareerCrafterDbContext(DbContextOptions<CareerCrafterDbContext> options)
            : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<JobSeeker> JobSeekers { get; set; }
        public DbSet<Application> Applications { get; set; }
        public DbSet<Resume> Resumes { get; set; }
        public DbSet<JobListing> JobListings { get; set; }
        public DbSet<UserInfo> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Primary Keys
            modelBuilder.Entity<Employee>().HasKey(e => e.EmployerId);
            modelBuilder.Entity<JobSeeker>().HasKey(j => j.JobSeekerId);
            modelBuilder.Entity<Application>().HasKey(a => a.ApplicationId);
            modelBuilder.Entity<Resume>().HasKey(r => r.ResumeId);
            modelBuilder.Entity<JobListing>().HasKey(j => j.JobListingId);
            modelBuilder.Entity<UserInfo>().HasKey(u => u.UserId);

            // Relationships
            modelBuilder.Entity<JobListing>()
                .HasOne(j => j.Employer)
                .WithMany(e => e.JobListings)
                .HasForeignKey(j => j.EmployerId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Application>()
                .HasOne(a => a.JobSeeker)
                .WithMany(j => j.Applications)
                .HasForeignKey(a => a.JobSeekerId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Application>()
                .HasOne(a => a.JobListing)
                .WithMany(j => j.Applications)
                .HasForeignKey(a => a.JobListingId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Resume>()
                .HasOne(r => r.JobSeeker)
                .WithOne(j => j.Resume)
                .HasForeignKey<Resume>(r => r.JobSeekerId)
                .OnDelete(DeleteBehavior.Cascade);

            // ✅ Added: UserInfo ➔ JobSeeker with CASCADE
            modelBuilder.Entity<JobSeeker>()
                .HasOne(js => js.User)
                .WithMany()
                .HasForeignKey(js => js.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Constraints
            modelBuilder.Entity<Resume>()
                .HasIndex(r => r.JobSeekerId)
                .IsUnique();

            modelBuilder.Entity<UserInfo>()
                .HasCheckConstraint("CK_UserInfo_Role", "Role IN ('Employer', 'JobSeeker')");
        }
    }
}
