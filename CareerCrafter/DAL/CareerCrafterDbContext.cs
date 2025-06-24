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

        // DbSets for each model class
        public DbSet<Employee> Employees { get; set; }
        public DbSet<JobSeeker> JobSeekers { get; set; }
        public DbSet<Application> Applications { get; set; }
        public DbSet<Resume> Resumes { get; set; }
        public DbSet<JobListing> JobListings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<Employee>().HasKey(e => e.EmployerId);
            modelBuilder.Entity<JobSeeker>().HasKey(j => j.JobSeekerId);
            modelBuilder.Entity<Application>().HasKey(a => a.ApplicationId);
            modelBuilder.Entity<Resume>().HasKey(r => r.ResumeId);
            modelBuilder.Entity<JobListing>().HasKey(j => j.JobListingId);


        }
    }
}
