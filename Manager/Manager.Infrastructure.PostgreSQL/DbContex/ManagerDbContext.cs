using Manager.Domain.Entites;
using Microsoft.EntityFrameworkCore;

namespace Manager.Infrastructure.PostgreSQL.DbContex
{
    public class ManagerDbContext : DbContext
    {
        public ManagerDbContext(DbContextOptions<ManagerDbContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TaskEntity>()
                .HasOne(t => t.User)
                .WithMany(u => u.Tasks)
                .HasForeignKey(t => t.UserId);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<TaskEntity> Tasks { get; set; }

    }
}
