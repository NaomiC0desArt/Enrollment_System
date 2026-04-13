
using Microsoft.EntityFrameworkCore;
using UniversitySystem.Application.Entities;
using UniversitySystem.Domain.Entities;
using UniversitySystem.Domain.Interfaces.Common;
using UniversitySystem.Domain.Interfaces.Repositories;
using UniversitySystem.Infrastructure.Data.Seed;

namespace UniversitySystem.Persistence.Data
{
    public class UniversityDbContext : DbContext
    {
        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<User> Users { get; set; }

        private ICurrentUserService _currentUserService;

        public UniversityDbContext(DbContextOptions<UniversityDbContext> options, ICurrentUserService userService) : base(options)
        {
            _currentUserService = userService;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region FluentApi Config
            modelBuilder.Entity<Student>(entity =>
            {
                entity.HasKey(s => s.Id);
                entity.Property(s => s.Name).IsRequired().HasMaxLength(100);
                entity.Property(s => s.Email).IsRequired().HasMaxLength(150);

                entity.HasIndex(s => s.Email).IsUnique();
                entity.HasQueryFilter(s => !s.IsDeleted);
            });

            modelBuilder.Entity<Course>(entity =>
            {
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Title).IsRequired().HasMaxLength(150);
                entity.Property(c => c.Credits).IsRequired();

                entity.HasIndex(c => c.Title).IsUnique();
            });

            modelBuilder.Entity<Enrollment>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Grade).HasDefaultValue(0);

                entity.HasOne(e => e.Student)
                .WithMany(s => s.Enrollments)
                .HasForeignKey(e => e.StudentId);

                entity.HasOne(e => e.Course)
                .WithMany(c => c.Enrollments)
                .HasForeignKey(e => e.CourseId);

                entity.HasQueryFilter(u => !u.IsDeleted);

            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.Property(u => u.Email).IsRequired().HasMaxLength(255);
                entity.Property(u => u.PasswordHash).IsRequired().HasMaxLength(255);

                entity.HasIndex(u => u.Email).IsUnique();

                entity.HasQueryFilter(u => !u.IsDeleted);

                entity.Property(u => u.Role)
                .HasConversion<string>()
                .HasMaxLength(20);
            });
            #endregion

            modelBuilder.Seed();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var userId = _currentUserService.UserId;

            foreach (var entry in ChangeTracker.Entries<IAuditable>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedAt = DateTime.UtcNow;
                        entry.Entity.CreatedBy = userId;
                        entry.Entity.IsDeleted = false;
                        break;

                    case EntityState.Modified:
                        entry.Entity.UpdatedAt = DateTime.UtcNow;
                        entry.Entity.UpdatedBy = userId;
                        break;

                    case EntityState.Deleted:
                        entry.State = EntityState.Modified;
                        entry.Entity.IsDeleted = true;
                        entry.Entity.UpdatedAt = DateTime.UtcNow;
                        entry.Entity.UpdatedBy = userId;
                        break;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
