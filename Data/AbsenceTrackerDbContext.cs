using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using absence_tracker.Models;

namespace absence_tracker.Data
{
    public class AbsenceTrackerDbContext : IdentityDbContext<User>
    {
        public AbsenceTrackerDbContext(DbContextOptions<AbsenceTrackerDbContext> options) : base(options)
        {
        }

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


    }
}
