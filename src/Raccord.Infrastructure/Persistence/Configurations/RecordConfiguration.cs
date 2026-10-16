using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Raccord.Modules.Compliance;

namespace Raccord.Infrastructure.Persistence.Configurations;

/// <summary>Append-only tables: issued invoices, audit events and compliance records.</summary>
internal sealed class RecordConfiguration
    : IEntityTypeConfiguration<IssuedInvoiceRow>,
        IEntityTypeConfiguration<AuditEvent>,
        IEntityTypeConfiguration<ComplianceRecordRow>,
        IEntityTypeConfiguration<InboundEInvoiceRow>
{
    public void Configure(EntityTypeBuilder<IssuedInvoiceRow> builder)
    {
        builder.ToTable("issued_invoices", "invoicing");
        builder.HasKey(row => row.Id);
        builder.Property(row => row.SeriesCode).HasMaxLength(10);
        builder.Property(row => row.Number).HasMaxLength(32);
        builder.Property(row => row.RecipientName).HasMaxLength(200);
        builder.Property(row => row.RecipientTaxId).HasMaxLength(32);
        builder.Property(row => row.Total).HasPrecision(18, 2);
        builder.Property(row => row.Currency).HasMaxLength(3);
        builder.Property(row => row.Representation).HasColumnType("text");
        builder.Property(row => row.ContentHash).HasMaxLength(64).IsFixedLength();
        builder.HasIndex(row => row.Number).IsUnique();
        builder
            .HasIndex(row => new
            {
                row.SeriesCode,
                row.Year,
                row.Sequence,
            })
            .IsUnique();
        builder.HasIndex(row => row.DraftId).IsUnique();
        builder.HasIndex(row => row.IssueDate);
    }

    public void Configure(EntityTypeBuilder<AuditEvent> builder)
    {
        builder.ToTable("audit_events", "compliance");
        builder.HasKey(item => item.Id);
        builder.Property(item => item.Id).ValueGeneratedNever();
        builder.Property(item => item.Actor).HasMaxLength(200);
        builder.Property(item => item.Action).HasMaxLength(100);
        builder.Property(item => item.EntityType).HasMaxLength(50);
        builder
            .Property(item => item.Details)
            .HasColumnType("jsonb")
            .HasConversion(
                details => JsonSerializer.Serialize(details, JsonSerializerOptions.Default),
                json =>
                    JsonSerializer.Deserialize<Dictionary<string, string>>(
                        json,
                        JsonSerializerOptions.Default
                    ) ?? new Dictionary<string, string>(StringComparer.Ordinal),
                new ValueComparer<IReadOnlyDictionary<string, string>>(
                    (left, right) =>
                        left != null
                        && right != null
                        && left.Count == right.Count
                        && !left.Except(right).Any(),
                    details =>
                        details.Aggregate(
                            0,
                            (hash, pair) => HashCode.Combine(hash, pair.Key, pair.Value)
                        ),
                    details => new Dictionary<string, string>(details, StringComparer.Ordinal)
                )
            );
        builder.HasIndex(item => new { item.EntityId, item.OccurredAt });
    }

    public void Configure(EntityTypeBuilder<ComplianceRecordRow> builder)
    {
        builder.ToTable("compliance_records", "compliance");
        builder.HasKey(row => row.Id);
        builder.Property(row => row.InvoiceNumber).HasMaxLength(32);
        builder.Property(row => row.ContentHash).HasMaxLength(64).IsFixedLength();
        builder.Property(row => row.Kind).HasMaxLength(30);
        builder.Property(row => row.Status).HasMaxLength(30);
        builder.Property(row => row.LastError).HasMaxLength(2000);
        builder.HasIndex(row => row.InvoiceId).IsUnique();
        builder.HasIndex(row => row.Status);
    }

    public void Configure(EntityTypeBuilder<InboundEInvoiceRow> builder)
    {
        builder.ToTable("inbound_e_invoices");
        builder.HasKey(row => row.Id);
        builder.Property(row => row.InvoiceNumber).HasMaxLength(64);
        builder.Property(row => row.SupplierName).HasMaxLength(200);
        builder.Property(row => row.SupplierTaxId).HasMaxLength(32);
        builder.Property(row => row.Currency).HasMaxLength(3);
        builder.Property(row => row.Status).HasMaxLength(20);
        builder.Property(row => row.Reason).HasMaxLength(2000);
        builder.Property(row => row.PayableAmount).HasPrecision(18, 2);
        builder.Property(row => row.ContentHash).HasMaxLength(64).IsFixedLength();
        builder.Property(row => row.Xml).HasColumnType("text");
        builder.HasIndex(row => row.ContentHash).IsUnique();
        builder.HasIndex(row => new { row.Status, row.StatusDueOn });
    }
}
