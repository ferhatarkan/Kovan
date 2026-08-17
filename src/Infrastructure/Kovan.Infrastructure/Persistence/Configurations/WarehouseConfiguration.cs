using Kovan.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kovan.Infrastructure.Persistence.Configurations;

public class WarehouseConfiguration : IEntityTypeConfiguration<Warehouse>
{
    public void Configure(EntityTypeBuilder<Warehouse> builder)
    {
        builder.HasKey(w => w.Id);

        builder.Property(w => w.Name)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(w => w.LocationAddress)
            .HasMaxLength(500);

        builder.Property(w => w.Type)
            .IsRequired();

        builder.HasMany(w => w.ProductWarehouses)
            .WithOne(pw => pw.Warehouse)
            .HasForeignKey(pw => pw.WarehouseId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
