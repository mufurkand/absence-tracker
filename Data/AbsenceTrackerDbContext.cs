using Microsoft.EntityFrameworkCore;
using absence_tracker.Models;

namespace absence_tracker.Data
{
    public class AbsenceTrackerDbContext : DbContext
    {
        public AbsenceTrackerDbContext(DbContextOptions<AbsenceTrackerDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Absence> Absences { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Add some seed data (optional)
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Seed users
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Username = "admin",
                    Email = "admin@example.com",
                    Password = "hashed_password_here" // In production, use proper password hashing
                }
            );

            // Seed courses
            modelBuilder.Entity<Course>().HasData(
                new Course
                {
                    Id = 1,
                    Name = "Mathematics",
                    Description = "Advanced Mathematics Course",
                    UserId = 1,
                    TotalAbsences = 0,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Course
                {
                    Id = 2,
                    Name = "Physics",
                    Description = "Introduction to Physics",
                    UserId = 1,
                    TotalAbsences = 0,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }
            );
        }
    }
}
