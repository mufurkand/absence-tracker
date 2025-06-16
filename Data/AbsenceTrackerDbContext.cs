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

            // Configure automatic timestamps for User entity
            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.UpdatedAt)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            // Configure automatic timestamps for Course entity
            modelBuilder.Entity<Course>(entity =>
            {
                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.UpdatedAt)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            // Configure automatic timestamps for Absence entity
            modelBuilder.Entity<Absence>(entity =>
            {
                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.UpdatedAt)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });
        }

        // normally postgres would need a trigger to update the UpdatedAt field, but we can handle
        // it in the SaveChanges method. ef migrations with triggers are not supported yet (?).
        public override int SaveChanges()
        {
            UpdateTimestamps();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateTimestamps();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void UpdateTimestamps()
        {
            var entries = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

            foreach (var entry in entries)
            {
                if (entry.Entity is User user)
                {
                    user.UpdatedAt = DateTime.UtcNow;
                }
                else if (entry.Entity is Course course)
                {
                    course.UpdatedAt = DateTime.UtcNow;
                }
                else if (entry.Entity is Absence absence)
                {
                    absence.UpdatedAt = DateTime.UtcNow;
                }
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            // Configure seeding using the recommended UseSeeding approach
            optionsBuilder.UseSeeding((context, _) =>
            {
                SeedData(context);
            });
        }

        private void SeedData(DbContext context)
        {
            var dbContext = (AbsenceTrackerDbContext)context;

            // Check if admin user already exists to avoid duplicates
            if (!dbContext.Users.Any(u => u.Username == "admin"))
            {
                // Seed admin user
                var adminUser = new User
                {
                    Username = "admin",
                    Email = "admin@example.com",
                    Password = "hashed_password_here" // In production, use proper password hashing
                };

                dbContext.Users.Add(adminUser);
                dbContext.SaveChanges();

                // Seed courses for the admin user
                var courses = new[]
                {
                    new Course
                    {
                        Name = "Mathematics",
                        Description = "Advanced Mathematics Course",
                        UserId = adminUser.Id,
                        TotalAbsences = 0
                    },
                    new Course
                    {
                        Name = "Physics",
                        Description = "Introduction to Physics",
                        UserId = adminUser.Id,
                        TotalAbsences = 0
                    }
                };

                dbContext.Courses.AddRange(courses);
                dbContext.SaveChanges();
            }
        }
    }
}
