using System.Text;
using Microsoft.EntityFrameworkCore;
using Raccord.Domain.Billing;
using Raccord.Domain.Commercial;
using Raccord.Domain.Organization;
using Raccord.Domain.Projects;
using Raccord.Modules.Compliance;

namespace Raccord.Infrastructure.Persistence;

/// <summary>The PostgreSQL system of record.</summary>
public sealed class RaccordDbContext(DbContextOptions<RaccordDbContext> options)
    : DbContext(options)
{
    internal DbSet<CustomerRow> Customers => Set<CustomerRow>();
    internal DbSet<ContactPersonRow> ContactPersons => Set<ContactPersonRow>();

    internal DbSet<Project> Projects => Set<Project>();
    internal DbSet<ProjectTemplate> ProjectTemplates => Set<ProjectTemplate>();

    internal DbSet<InvoiceDraft> InvoiceDrafts => Set<InvoiceDraft>();

    internal DbSet<InvoiceSeries> InvoiceSeries => Set<InvoiceSeries>();

    internal DbSet<InvoiceNumberSequence> InvoiceNumberSequences => Set<InvoiceNumberSequence>();

    internal DbSet<InvoicePayment> InvoicePayments => Set<InvoicePayment>();
    internal DbSet<InvoiceReminder> InvoiceReminders => Set<InvoiceReminder>();

    internal DbSet<IssuerProfile> IssuerProfiles => Set<IssuerProfile>();
    internal DbSet<FinancialSettings> FinancialSettings => Set<FinancialSettings>();
    internal DbSet<Quote> Quotes => Set<Quote>();
    internal DbSet<QuoteNumberSequence> QuoteNumberSequences => Set<QuoteNumberSequence>();

    internal DbSet<IssuedInvoiceRow> IssuedInvoices => Set<IssuedInvoiceRow>();

    internal DbSet<AuditEvent> AuditEvents => Set<AuditEvent>();

    internal DbSet<ComplianceRecordRow> ComplianceRecords => Set<ComplianceRecordRow>();
    internal DbSet<EquipmentItemRow> EquipmentItems => Set<EquipmentItemRow>();
    internal DbSet<EquipmentStockMovementRow> EquipmentStockMovements =>
        Set<EquipmentStockMovementRow>();
    internal DbSet<LetterheadRow> Letterheads => Set<LetterheadRow>();
    internal DbSet<CommunicationRow> Communications => Set<CommunicationRow>();
    internal DbSet<CrewMemberRow> CrewMembers => Set<CrewMemberRow>();
    internal DbSet<VehicleRow> Vehicles => Set<VehicleRow>();
    internal DbSet<CrewRateRow> CrewRates => Set<CrewRateRow>();
    internal DbSet<ProjectFunctionRow> ProjectFunctions => Set<ProjectFunctionRow>();
    internal DbSet<CrewAvailabilityRow> CrewAvailability => Set<CrewAvailabilityRow>();
    internal DbSet<CrewInvitationRow> CrewInvitations => Set<CrewInvitationRow>();
    internal DbSet<CrewJobRow> CrewJobs => Set<CrewJobRow>();

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ArgumentNullException.ThrowIfNull(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(RaccordDbContext).Assembly);
        ApplySnakeCaseNames(modelBuilder);
    }

    private static void ApplySnakeCaseNames(ModelBuilder modelBuilder)
    {
        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            foreach (var property in entity.GetProperties())
            {
                property.SetColumnName(ToSnakeCase(property.GetColumnName()));
            }

            foreach (var complex in entity.GetComplexProperties())
            {
                foreach (var property in complex.ComplexType.GetProperties())
                {
                    property.SetColumnName(ToSnakeCase(property.GetColumnName()));
                }
            }

            foreach (var key in entity.GetKeys())
            {
                key.SetName(ToSnakeCase(key.GetName() ?? string.Empty));
            }

            foreach (var foreignKey in entity.GetForeignKeys())
            {
                foreignKey.SetConstraintName(
                    ToSnakeCase(foreignKey.GetConstraintName() ?? string.Empty)
                );
            }

            foreach (var index in entity.GetIndexes())
            {
                index.SetDatabaseName(ToSnakeCase(index.GetDatabaseName() ?? string.Empty));
            }
        }
    }

    private static string ToSnakeCase(string name)
    {
        var builder = new StringBuilder(name.Length + 8);
        for (var i = 0; i < name.Length; i++)
        {
            var current = name[i];
            if (char.IsUpper(current))
            {
                var previous = i > 0 ? name[i - 1] : '_';
                var startsWord =
                    previous != '_'
                    && (
                        char.IsLower(previous)
                        || char.IsDigit(previous)
                        || (i + 1 < name.Length && char.IsLower(name[i + 1]))
                    );
                if (startsWord)
                {
                    builder.Append('_');
                }

                builder.Append(char.ToLowerInvariant(current));
            }
            else
            {
                builder.Append(current);
            }
        }

        return builder.ToString();
    }
}
