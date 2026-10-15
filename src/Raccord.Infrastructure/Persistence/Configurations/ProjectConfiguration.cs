using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Raccord.Domain.Projects;

namespace Raccord.Infrastructure.Persistence.Configurations;

internal sealed class ProjectConfiguration : IEntityTypeConfiguration<Project>
{
#pragma warning disable MA0051
    public void Configure(EntityTypeBuilder<Project> builder)
    {
        builder.ToTable("projects");
        builder.HasKey(project => project.Id);
        builder.Property(project => project.Name).HasMaxLength(200);
        builder.Property(project => project.DefaultDiscountPercent).HasPrecision(5, 2);
        builder.Property(project => project.PaymentTerms).HasMaxLength(200);
        builder.Property(project => project.RefundableDepositAmount).HasPrecision(18, 2);
        builder.Property(project => project.DepositStatus).HasConversion<string>().HasMaxLength(20);
        builder.Property(project => project.Status).HasConversion<string>().HasMaxLength(20);
        builder.HasConcurrencyToken();

        builder.OwnsMany(
            project => project.BillableLines,
            line =>
            {
                line.ToTable("billable_lines");
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
            project => project.PlannedEquipmentLines,
            line =>
            {
                line.ToTable("planned_equipment_lines");
                line.WithOwner().HasForeignKey("ProjectId");
                line.HasKey(item => item.Id);
                line.Property(item => item.Id).ValueGeneratedNever();
            }
        );
        builder
            .Navigation(project => project.PlannedEquipmentLines)
            .UsePropertyAccessMode(PropertyAccessMode.Field);
        builder.OwnsMany(
            project => project.PackingLines,
            line =>
            {
                line.ToTable("packing_lines");
                line.WithOwner().HasForeignKey("ProjectId");
                line.HasKey(item => item.Id);
                line.Property(item => item.Id).ValueGeneratedNever();
                line.Property(item => item.PackedBy).HasMaxLength(200);
                line.HasConcurrencyToken();
            }
        );
        builder
            .Navigation(project => project.PackingLines)
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
#pragma warning restore MA0051
}
