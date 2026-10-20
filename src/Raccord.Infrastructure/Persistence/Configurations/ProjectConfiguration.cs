using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Raccord.Domain.Projects;

namespace Raccord.Infrastructure.Persistence.Configurations;

internal sealed class ProjectConfiguration : IEntityTypeConfiguration<Project>
{
#pragma warning disable MA0051
    public void Configure(EntityTypeBuilder<Project> builder)
    {
        builder.ToTable("projects", "projects");
        builder.HasKey(project => project.Id);
        builder.Property(project => project.Name).HasMaxLength(200);
        builder.Property(project => project.ExternalReference).HasMaxLength(200);
        builder.Property(project => project.ProjectTypeId);
        builder.Property(project => project.TeamId);
        builder
            .Property(project => project.Status)
            .HasConversion<string>()
            .HasMaxLength(30)
            .HasDefaultValue(ProjectStatus.Draft);
        builder.Property(project => project.HasCancellationWarning).IsRequired();
        builder.Property(project => project.CancelledAt);
        builder.Property(project => project.ProgressPercent).HasPrecision(5, 2).IsRequired();
        builder.Property(project => project.RefundableDepositAmount).HasPrecision(18, 2);
        builder.Property(project => project.DepositStatus).HasConversion<string>().HasMaxLength(20);
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
                line.Property(packing => packing.Role).HasConversion<string>().HasMaxLength(20);
                line.Property(packing => packing.Disposition)
                    .HasConversion<string>()
                    .HasMaxLength(20);
                line.Property(packing => packing.ExceptionKind).HasMaxLength(50);
                line.Property(packing => packing.ExceptionDescription).HasMaxLength(500);
                line.Property(packing => packing.ExceptionReportedBy).HasMaxLength(200);
                line.Property(packing => packing.PackedBy).HasMaxLength(200);
                line.Property(packing => packing.BookedOutBy).HasMaxLength(200);
                line.Property(packing => packing.ReceivedBy).HasMaxLength(200);
            }
        );
        builder
            .Navigation(project => project.PackingLines)
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.OwnsMany(
            project => project.TimePeriods,
            period =>
            {
                period.ToTable(
                    "project_time_periods",
                    "projects",
                    table =>
                        table.HasCheckConstraint(
                            "CK_project_time_periods_complete_range",
                            "(\"start\" IS NULL AND \"end\" IS NULL) OR (\"start\" IS NOT NULL AND \"end\" IS NOT NULL AND \"end\" > \"start\")"
                        )
                );
                period.WithOwner().HasForeignKey("ProjectId");
                period.HasKey(timePeriod => timePeriod.Id);
                period.Property(timePeriod => timePeriod.Id).ValueGeneratedNever();
                period.Property(timePeriod => timePeriod.Name).HasMaxLength(120).IsRequired();
                period.Property(timePeriod => timePeriod.Start);
                period.Property(timePeriod => timePeriod.End);
                period
                    .Property(timePeriod => timePeriod.ShowOnInternalDocuments)
                    .HasDefaultValue(true)
                    .IsRequired();
                period
                    .Property(timePeriod => timePeriod.ShowOnExternalDocuments)
                    .HasDefaultValue(false)
                    .IsRequired();
                period.HasIndex("ProjectId", nameof(ProjectTimePeriod.Position)).IsUnique();
                period
                    .HasIndex("ProjectId", nameof(ProjectTimePeriod.IsDefaultPlanning))
                    .IsUnique()
                    .HasFilter("is_default_planning");
                period
                    .HasIndex("ProjectId", nameof(ProjectTimePeriod.IsDefaultUsage))
                    .IsUnique()
                    .HasFilter("is_default_usage");
            }
        );
        builder
            .Navigation(project => project.TimePeriods)
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
#pragma warning restore MA0051
}
