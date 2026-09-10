using Kovan.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kovan.Infrastructure.Persistence.Configurations;

public class InvoiceLineConfiguration : IEntityTypeConfiguration<InvoiceLine>
{
    public void Configure(EntityTypeBuilder<InvoiceLine> builder)
    {
        builder.HasKey(il => il.Id);

        builder.Property(il => il.Quantity)
            .HasColumnType("decimal(18,4)");

        builder.Property(il => il.UnitPrice)
            .HasColumnType("decimal(18,2)");

        // builder.Property(il => il.LineTotal)
        //     .HasColumnType("decimal(18,2)");
        //
        // builder.Property(il => il.Description)
        //     .HasMaxLength(500);
        //
        // builder.HasOne(il => il.Invoice)
        //     .WithMany(i => i.Lines)
        //     .HasForeignKey(il => il.InvoiceId)
        //     .OnDelete(DeleteBehavior.Cascade);
    }
}
