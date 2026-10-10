using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Raccord.Domain.Customers;
using Raccord.Domain.Projects;

namespace Raccord.Infrastructure.Persistence.Configurations;

internal sealed class ProjectConfiguration : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> builder)
    {
        builder.ToTable("projects");
        builder.HasKey(project => project.Id);
        builder.Property(project => project.Name).HasMaxLength(200);
        builder
            .HasOne<Customer>()
            .WithMany()
            .HasForeignKey(project => project.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);
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
    }
}
