using Microsoft.EntityFrameworkCore;
using Shared.Models;

namespace ConversionService.Data
{
    public class ConversionDbContext : DbContext
    {
        public ConversionDbContext(DbContextOptions<ConversionDbContext> options) : base(options) { }

        public DbSet<QuantityMeasurementEntity> QuantityMeasurements { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<QuantityMeasurementEntity>(entity =>
            {
                entity.ToTable("QuantityMeasurements");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.OperationType).IsRequired().HasMaxLength(50);
                entity.Property(e => e.MeasurementType).IsRequired().HasMaxLength(50);
                entity.Property(e => e.FirstUnit).IsRequired().HasMaxLength(50);
                entity.Property(e => e.SecondUnit).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Result).IsRequired().HasMaxLength(200);
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.HasIndex(e => e.OperationType).HasDatabaseName("IX_QuantityMeasurements_OperationType");
                entity.HasIndex(e => e.MeasurementType).HasDatabaseName("IX_QuantityMeasurements_MeasurementType");

                // UserId is a soft FK — no navigation needed in this service
                entity.Ignore(e => e.User);
            });
        }
    }
}
