using Microsoft.EntityFrameworkCore;
using TaskManagementApi.DAL.Models;

namespace TaskManagementApi.DAL.Data
{
    public class TaskDbContext : DbContext
    {
        public TaskDbContext(DbContextOptions<TaskDbContext> options) : base(options)
        {
        }

        public DbSet<TaskItem> Tasks { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TaskItem>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Description).HasMaxLength(1000);

                // Audit and assignment fields
                entity.Property(e => e.AssigneeUserId).IsRequired();
                entity.Property(e => e.CreatedByUserId).IsRequired();
                entity.Property(e => e.UpdatedByUserId);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Username).IsRequired().HasMaxLength(100);
                entity.Property(e => e.PasswordHash).IsRequired();
                entity.HasIndex(e => e.Username).IsUnique();
            });

            // Seed sample data
            modelBuilder.Entity<TaskItem>().HasData(TaskDataSeeder.GetSeedData());
            modelBuilder.Entity<User>().HasData(UserDataSeeder.GetSeedData());
        }
    }
}
