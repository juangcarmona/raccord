using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Raccord.Infrastructure.Persistence.Configurations;

internal sealed class ContactTagAssignmentConfiguration
    : IEntityTypeConfiguration<ContactTagAssignmentRow>
{
    public void Configure(EntityTypeBuilder<ContactTagAssignmentRow> builder)
    {
        builder.ToTable("contact_tag_assignments", "contacts");
        builder.HasKey(assignment => new { assignment.ContactId, assignment.TagId });
        builder
            .HasOne(assignment => assignment.Contact)
            .WithMany()
            .HasForeignKey(assignment => assignment.ContactId)
            .OnDelete(DeleteBehavior.Cascade);
        builder
            .HasOne(assignment => assignment.Tag)
            .WithMany(tag => tag.Assignments)
            .HasForeignKey(assignment => assignment.TagId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasIndex(assignment => assignment.TagId);
    }
}
