using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Raccord.Domain.Projects;

namespace Raccord.Infrastructure.Persistence.Configurations;

internal sealed class ProjectConfiguration : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> builder)
    {
        builder.ToTable("projects", "projects");
        builder.HasKey(project => project.Id);
        builder.Property(project => project.Name).HasMaxLength(200);
        builder.Property(project => project.ProjectTypeId);
        builder.Property(project => project.TeamId);
        builder.Property(project => project.ProgressPercent).HasPrecision(5, 2).IsRequired();
        builder.HasConcurrencyToken();

        builder.OwnsMany(
            project => project.BillableLines,
            line =>
            {
                line.ToTable("billable_lines", "projects");
                line.WithOwner().HasForeignKey("ProjectId");
                line.HasKey(billable => billable.Id);
                line.Property(billable => billable.Id).ValueGeneratedNever();
                line.Property(billable => billable.Kind).HasConversion<string>().HasMaxLength(20);
                line.Property(billable => billable.State).HasConversion<string>().HasMaxLength(20);
                line.Property(billable => billable.Description).HasMaxLength(500);
                line.Property(billable => billable.Quantity).HasPrecision(18, 4);
                line.Property(billable => billable.UnitPrice).HasPrecision(18, 4);
                line.Property(billable => billable.DiscountPercent).HasPrecision(5, 2);
                line.Property(billable => billable.VatPercentage).HasPrecision(5, 2);
                line.Property(billable => billable.CurrencyCode).HasMaxLength(3);
                line.HasIndex(billable => billable.DraftId);
                line.HasIndex("ProjectId", nameof(BillableLine.Position)).IsUnique();
                line.HasConcurrencyToken();
            }
        );
        builder
            .Navigation(project => project.BillableLines)
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.OwnsMany(
            project => project.PackingLines,
            line =>
            {
                line.ToTable("packing_lines", "projects");
                line.WithOwner().HasForeignKey("ProjectId");
                line.HasKey(packing => packing.Id);
                line.Property(packing => packing.Id).ValueGeneratedNever();
                line.Property(packing => packing.EquipmentId).IsRequired();
                line.Property(packing => packing.Quantity).IsRequired();
                line.Property(packing => packing.PackedBy).HasMaxLength(200);
                line.Property(packing => packing.BookedOutBy).HasMaxLength(200);
                line.Property(packing => packing.ReceivedBy).HasMaxLength(200);
            }
        );
        builder
            .Navigation(project => project.PackingLines)
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
