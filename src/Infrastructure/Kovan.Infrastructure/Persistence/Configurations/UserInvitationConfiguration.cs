using Kovan.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kovan.Infrastructure.Persistence.Configurations;

public class UserInvitationConfiguration : IEntityTypeConfiguration<UserInvitation>
{
    public void Configure(EntityTypeBuilder<UserInvitation> builder)
    {
        builder.HasKey(ui => ui.Id);

        builder.Property(ui => ui.Email)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(ui => ui.InvitationToken)
            .IsRequired();

        builder.Property(ui => ui.ExpiresAt)
            .IsRequired();

        builder.Property(ui => ui.IsAccepted)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(ui => ui.InvitedByUserId)
            .IsRequired()
            .HasMaxLength(255);
    }
}
