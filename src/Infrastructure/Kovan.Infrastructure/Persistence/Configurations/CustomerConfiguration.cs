using Kovan.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kovan.Infrastructure.Persistence.Configurations;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.FirstName)
            .HasMaxLength(255);

        builder.Property(c => c.LastName)
            .HasMaxLength(255);

        builder.Property(c => c.CustomerType)
            .IsRequired();

        builder.Property(c => c.Title)
            .HasMaxLength(255);

        builder.Property(c => c.TaxOffice)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(c => c.TaxNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(c => c.NationalIdentityNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(c => c.Address)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(c => c.PhoneNumber)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(c => c.Email)
            .HasMaxLength(255);
    }
}
