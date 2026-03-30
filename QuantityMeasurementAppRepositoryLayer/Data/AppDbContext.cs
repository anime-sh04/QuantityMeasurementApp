using Microsoft.EntityFrameworkCore;
using QuantityMeasurementAppModelLayer.Models;

namespace QuantityMeasurementAppRepositoryLayer.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<QuantityMeasurementEntity> QuantityMeasurements { get; set; }
        public DbSet<ApplicationUser>           Users                { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUser>(entity =>
            {
                entity.ToTable("Users");
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Email).IsUnique();
                entity.HasIndex(e => e.GoogleId);

                entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
                entity.Property(e => e.FirstName).HasMaxLength(100);
                entity.Property(e => e.LastName).HasMaxLength(100);
                entity.Property(e => e.GoogleId).HasMaxLength(200);
                entity.Property(e => e.Role).IsRequired().HasMaxLength(20).HasDefaultValue("User");
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            });

            // ── QuantityMeasurementEntity ─────────────────────────────────────
            modelBuilder.Entity<QuantityMeasurementEntity>(entity =>
            {
                entity.ToTable("QuantityMeasurements");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.OperationType).IsRequired().HasMaxLength(50);
                entity.Property(e => e.MeasurementType).IsRequired().HasMaxLength(50);
                entity.Property(e => e.FirstUnit).IsRequired().HasMaxLength(50);
                entity.Property(e => e.SecondUnit).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Result).IsRequired().HasMaxLength(200);
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");

                entity.HasIndex(e => e.OperationType).HasDatabaseName("IX_QuantityMeasurements_OperationType");
                entity.HasIndex(e => e.MeasurementType).HasDatabaseName("IX_QuantityMeasurements_MeasurementType");

                entity.HasOne(e => e.User)
                      .WithMany(u => u.Measurements)
                      .HasForeignKey(e => e.UserId)
                      .OnDelete(DeleteBehavior.SetNull)
                      .IsRequired(false);
            });
        }
    }
}
