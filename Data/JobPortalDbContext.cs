using JobPortal.Models;
using Microsoft.EntityFrameworkCore;

namespace JobPortal.Data
{
    public class JobPortalDbContext : DbContext
    {
        public JobPortalDbContext(DbContextOptions<JobPortalDbContext> options)
            : base(options)
        {
        }

        public DbSet<Job> Jobs { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<JobSeeker> JobSeekers { get; set; }
        public DbSet<JobApplication> JobApplications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure relationships
            modelBuilder.Entity<Job>(entity =>
            {
                entity.HasOne(d => d.CompanyNavigation)
                    .WithMany(p => p.Jobs)
                    .HasForeignKey(d => d.CompanyId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Jobs_Companies");
            });

            modelBuilder.Entity<JobApplication>(entity =>
            {
                entity.HasOne(d => d.Job)
                    .WithMany()
                    .HasForeignKey(d => d.JobId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_JobApplications_Jobs");

                entity.HasOne(d => d.JobSeeker)
                    .WithMany()
                    .HasForeignKey(d => d.JobSeekerId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_JobApplications_JobSeekers");
            });

            // Seed data (optional)
            modelBuilder.Entity<Company>().HasData(
                new Company
                {
                    Id = 1,
                    Name = "Tech Solutions Inc.",
                    Description = "Leading software development company",
                    Industry = "Information Technology",
                    Website = "https://techsolutions.example.com",
                    Location = "New York, NY",
                    Email = "info@techsolutions.example.com",
                    FoundedDate = new System.DateTime(2010, 1, 1)
                }
            );

            modelBuilder.Entity<Job>().HasData(
                new Job
                {
                    Id = 1,
                    Title = "Senior Software Engineer",
                    Description = "We are looking for an experienced software engineer with expertise in .NET Core and React.",
                    Company = "Tech Solutions Inc.",
                    Location = "New York, NY",
                    JobType = "Full-time",
                    Salary = 120000,
                    PostedDate = System.DateTime.Now.AddDays(-10),
                    Deadline = System.DateTime.Now.AddDays(20),
                    IsActive = true,
                    CompanyId = 1
                }
            );
        }
    }
} 