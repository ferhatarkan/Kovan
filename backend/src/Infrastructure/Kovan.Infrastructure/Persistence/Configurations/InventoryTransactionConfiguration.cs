using Kovan.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kovan.Infrastructure.Persistence.Configurations;

public class InventoryTransactionConfiguration : IEntityTypeConfiguration<InventoryTransaction>
{
    public void Configure(EntityTypeBuilder<InventoryTransaction> builder)
    {
        builder.HasKey(it => it.Id);

        builder.Property(it => it.QuantityChanged)
            .HasColumnType("decimal(18,4)");

        builder.Property(it => it.ReferenceId)
            .HasMaxLength(100);

        // builder.Property(it => it.Notes)
        //     .HasMaxLength(500);

        builder.HasOne(it => it.Product)
            .WithMany()
            .HasForeignKey(it => it.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(it => it.Warehouse)
            .WithMany()
            .HasForeignKey(it => it.WarehouseId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
