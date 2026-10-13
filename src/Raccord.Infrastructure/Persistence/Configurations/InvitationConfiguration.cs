using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Raccord.Infrastructure.Persistence.Configurations;

internal sealed class InvitationConfiguration : IEntityTypeConfiguration<Invitation>
{
    public void Configure(EntityTypeBuilder<Invitation> builder)
    {
        builder.ToTable("invitations", "workspace");
        builder.HasKey(invitation => invitation.Id);
        builder.Property(invitation => invitation.Email).HasMaxLength(320).IsRequired();
        builder.Property(invitation => invitation.RoleName).HasMaxLength(256).IsRequired();
        builder.Property(invitation => invitation.TokenHash).HasMaxLength(64).IsRequired();
        builder.HasIndex(invitation => invitation.TokenHash).IsUnique();
    }
}
