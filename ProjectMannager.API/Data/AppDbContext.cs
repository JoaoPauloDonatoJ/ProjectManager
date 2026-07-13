using Microsoft.EntityFrameworkCore;
using ProjectMannager.API.Entities;
using ProjectMannager.API.Infrastructure;

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
        public DbSet<Column> Columns => Set<Column>();

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is BaseEntity && (e.State == EntityState.Added || e.State == EntityState.Deleted));

            foreach (var entry in entries)
            {
                var entity = (BaseEntity)entry.Entity;

                if (entry.State == EntityState.Added)
                {
                    entity.CreatedAt = DateTime.UtcNow;
                    entity.StateCode = 1;
                }
                else if (entry.State == EntityState.Deleted)
                {
                    entry.State = EntityState.Modified;
                    entity.IsDeleted = true;
                    entity.DeletedAt = DateTime.UtcNow;
                    entity.StateCode = 0;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // 1. Executa a base do framework
            base.OnModelCreating(modelBuilder);

            // 2. Suas configurações manuais de Fluent API (User, Workspace, Board)
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

                entity.HasOne(w => w.User)
                    .WithMany(u => u.Workspaces)
                    .HasForeignKey(w => w.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Board>(entity =>
            {
                entity.Property(b => b.Name).IsRequired().HasMaxLength(100);

                entity.HasOne(b => b.Workspace)
                    .WithMany(w => w.Boards)
                    .HasForeignKey(b => b.WorkspaceId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Column>(entity =>
            {
                entity.Property(c => c.Name).IsRequired().HasMaxLength(100);

                entity.HasOne(c => c.Board)
                    .WithMany(b => b.Columns)
                    .HasForeignKey(c => c.BoardId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // 3. Loop automático para injetar o Soft Delete (HasQueryFilter) em quem herda de BaseEntity
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
                {
                    modelBuilder.Entity(entityType.ClrType)
                        .HasQueryFilter(ConvertFilterExpression(entityType.ClrType));
                }
            }
        }

        private static System.Linq.Expressions.LambdaExpression ConvertFilterExpression(Type type)
        {
            var parameter = System.Linq.Expressions.Expression.Parameter(type, "e");
            var property = System.Linq.Expressions.Expression.Property(parameter, nameof(BaseEntity.IsDeleted));
            var falseConstant = System.Linq.Expressions.Expression.Constant(false);
            var comparison = System.Linq.Expressions.Expression.Equal(property, falseConstant);

            return System.Linq.Expressions.Expression.Lambda(comparison, parameter);
        }
    }
}