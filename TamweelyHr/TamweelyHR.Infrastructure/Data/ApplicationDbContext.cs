using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TamweelyHr.Domain.Entities;

namespace TamweelyHR.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Job> JobTitles { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Employee>(entity =>
            {
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(e => e.Email).HasMaxLength(256);
                entity.Property(e => e.PhoneNumber).HasMaxLength(20);
                entity.Property(e => e.FirstName).HasMaxLength(100).IsRequired();
                entity.Property(e => e.LastName).HasMaxLength(100).IsRequired();

                entity.HasOne(e => e.Department)
                    .WithMany(d => d.Employees)
                    .HasForeignKey(e => e.DepartmentId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Job)
                    .WithMany(j => j.Employees)
                    .HasForeignKey(e => e.JobId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<Department>(entity =>
            {
                entity.HasIndex(d => d.Name).IsUnique();
                entity.Property(d => d.Name).HasMaxLength(100).IsRequired();
            });

            builder.Entity<Job>(entity =>
            {
                entity.HasIndex(j => j.Title).IsUnique();
                entity.Property(j => j.Title).HasMaxLength(100).IsRequired();
            });
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker.Entries();
            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Modified)
                {
                    if (entry.Entity is BaseEntity baseEntity)
                    {
                        baseEntity.UpdatedAt = DateTime.UtcNow;
                    }
                }
            }
            return await base.SaveChangesAsync(cancellationToken);
        }

    }
}
