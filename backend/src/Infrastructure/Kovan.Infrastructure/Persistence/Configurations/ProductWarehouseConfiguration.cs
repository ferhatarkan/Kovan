using Kovan.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kovan.Infrastructure.Persistence.Configurations;

public class ProductWarehouseConfiguration : IEntityTypeConfiguration<ProductWarehouse>
{
    public void Configure(EntityTypeBuilder<ProductWarehouse> builder)
    {
        builder.HasKey(pw => pw.Id);

        builder.Property(pw => pw.StockQuantity)
            .HasColumnType("decimal(18,4)");

        // builder.Property(pw => pw.ReorderPoint)
        //     .HasColumnType("decimal(18,4)");
        //
        // builder.Property(pw => pw.ReorderQuantity)
        //     .HasColumnType("decimal(18,4)");

        builder.HasOne(pw => pw.Product)
            .WithMany(p => p.ProductWarehouses)
            .HasForeignKey(pw => pw.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(pw => pw.Warehouse)
            .WithMany()
            .HasForeignKey(pw => pw.WarehouseId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(pw => new { pw.ProductId, pw.WarehouseId })
            .IsUnique();
    }
}
