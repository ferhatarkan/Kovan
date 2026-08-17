using Kovan.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kovan.Infrastructure.Persistence.Configurations;

public class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
{
    public void Configure(EntityTypeBuilder<Invoice> builder)
    {
        builder.HasKey(i => i.Id);

        builder.Property(i => i.InvoiceNumber)
            .IsRequired()
            .HasMaxLength(50);

        // builder.Property(i => i.PaidAmount)
        //     .HasColumnType("decimal(18,2)");
        //
        // builder.Property(i => i.Notes)
        //     .HasMaxLength(1000);

        builder.HasOne(i => i.Customer)
            .WithMany(c => c.Invoices)
            .HasForeignKey(i => i.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        // builder.HasMany(i => i.Lines)
        //     .WithOne(il => il.Invoice)
        //     .HasForeignKey(il => il.InvoiceId)
        //     .OnDelete(DeleteBehavior.Cascade);
        //
        // builder.HasMany(i => i.Payments)
        //     .WithOne(p => p.Invoice)
        //     .HasForeignKey(p => p.InvoiceId)
        //     .OnDelete(DeleteBehavior.Cascade);
    }
}
