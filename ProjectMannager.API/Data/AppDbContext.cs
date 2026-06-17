using Microsoft.EntityFrameworkCore;
using ProjectMannager.API.Entities;

namespace ProjectMannager.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<User> Users => Set<User>();
        public DbSet<Workspace> Workspaces => Set<Workspace>();
        public DbSet<Board> Boards => Set<Board>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().HasKey(u => u.Id);

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email).IsUnique();

            modelBuilder.Entity<User>()
                .Property(u => u.UserName)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<User>()
                .Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(150);

            modelBuilder.Entity<User>()
                .Property(u => u.PasswordHash)
                .IsRequired()
                .HasMaxLength(255);

            modelBuilder.Entity<Workspace>(entity =>
            {
                entity.Property(w => w.Name).IsRequired().HasMaxLength(100);
                entity.Property(w => w.Description).HasMaxLength(500);

                // Relacionamento User (1) -> Workspace (N)
                entity.HasOne(w => w.User)
                    .WithMany(u => u.Workspaces)
                    .HasForeignKey(w => w.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Board>(entity =>
            {
                entity.Property(b => b.Name).IsRequired().HasMaxLength(100);

                // Relacionamento Workspace (1) -> Board (N)
                entity.HasOne(b => b.Workspace)
                    .WithMany(w => w.Boards)
                    .HasForeignKey(b => b.WorkspaceId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

        }
    }
}
