using Kovan.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kovan.Infrastructure.Persistence.Configurations;

public class PurchaseOrderLineConfiguration : IEntityTypeConfiguration<PurchaseOrderLine>
{
    public void Configure(EntityTypeBuilder<PurchaseOrderLine> builder)
    {
        builder.HasKey(pol => pol.Id);

        builder.Property(pol => pol.Quantity)
            .HasColumnType("decimal(18,4)");

        // builder.Property(pol => pol.UnitPrice)
        //     .HasColumnType("decimal(18,2)");
        //
        // builder.Property(pol => pol.LineTotal)
        //     .HasColumnType("decimal(18,2)");
        //
        // builder.Property(pol => pol.Description)
        //     .HasMaxLength(500);

        builder.HasOne(pol => pol.PurchaseOrder)
            .WithMany(po => po.Lines)
            .HasForeignKey(pol => pol.PurchaseOrderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(pol => pol.Product)
            .WithMany()
            .HasForeignKey(pol => pol.ProductId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
