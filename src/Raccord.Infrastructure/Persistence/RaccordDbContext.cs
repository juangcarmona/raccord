using System.Text;
using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Raccord.Domain.Billing;
using Raccord.Domain.Equipment;
using Raccord.Domain.Projects;
using Raccord.Modules.Compliance;

namespace Raccord.Infrastructure.Persistence;

/// <summary>The PostgreSQL system of record.</summary>
public sealed class RaccordDbContext(DbContextOptions<RaccordDbContext> options)
    : IdentityDbContext<RaccordUser, IdentityRole<Guid>, Guid>(options),
        IDataProtectionKeyContext
{
    /// <summary>Database-backed data-protection keys used to keep sessions across restarts.</summary>
    public DbSet<DataProtectionKey> DataProtectionKeys { get; set; } = null!;

    internal DbSet<CustomerRow> Customers => Set<CustomerRow>();

    internal DbSet<Project> Projects => Set<Project>();

    internal DbSet<ProjectRequest> ProjectRequests => Set<ProjectRequest>();

    internal DbSet<ProjectType> ProjectTypes => Set<ProjectType>();

    internal DbSet<ProjectTeam> ProjectTeams => Set<ProjectTeam>();

    internal DbSet<ProjectTask> ProjectTasks => Set<ProjectTask>();

    internal DbSet<ProjectTaskStatus> ProjectTaskStatuses => Set<ProjectTaskStatus>();

    internal DbSet<ProjectSchedule> ProjectSchedules => Set<ProjectSchedule>();

    internal DbSet<ProjectContextEntry> ProjectContextEntries => Set<ProjectContextEntry>();

    internal DbSet<ProjectTimeEntry> ProjectTimeEntries => Set<ProjectTimeEntry>();

    internal DbSet<InvoiceDraft> InvoiceDrafts => Set<InvoiceDraft>();

    internal DbSet<InvoiceNumberSequence> InvoiceNumberSequences => Set<InvoiceNumberSequence>();

    internal DbSet<InvoiceSeriesRow> InvoiceSeries => Set<InvoiceSeriesRow>();

    internal DbSet<IssuerProfileRow> IssuerProfiles => Set<IssuerProfileRow>();

    internal DbSet<IssuedInvoiceRow> IssuedInvoices => Set<IssuedInvoiceRow>();

    internal DbSet<InvoicePayment> InvoicePayments => Set<InvoicePayment>();

    internal DbSet<InvoiceReminder> InvoiceReminders => Set<InvoiceReminder>();

    internal DbSet<AuditEvent> AuditEvents => Set<AuditEvent>();

    internal DbSet<EquipmentItem> EquipmentItems => Set<EquipmentItem>();

    internal DbSet<StockMovement> StockMovements => Set<StockMovement>();

    internal DbSet<StockCount> StockCounts => Set<StockCount>();

    internal DbSet<ComplianceRecordRow> ComplianceRecords => Set<ComplianceRecordRow>();

    internal DbSet<DocumentTemplateRow> DocumentTemplates => Set<DocumentTemplateRow>();

    internal DbSet<CommunicationRow> Communications => Set<CommunicationRow>();

    internal DbSet<CrewMemberRow> CrewMembers => Set<CrewMemberRow>();

    internal DbSet<VehicleRow> Vehicles => Set<VehicleRow>();

    internal DbSet<CrewRateRow> CrewRates => Set<CrewRateRow>();

    internal DbSet<CrewAvailabilityRow> CrewAvailability => Set<CrewAvailabilityRow>();

    internal DbSet<CrewInvitationRow> CrewInvitations => Set<CrewInvitationRow>();

    internal DbSet<CrewJobRow> CrewJobs => Set<CrewJobRow>();

    internal DbSet<CrewSignupRow> CrewSignups => Set<CrewSignupRow>();

    internal DbSet<ProjectFunctionRow> ProjectFunctions => Set<ProjectFunctionRow>();

    internal DbSet<CrewHoursRow> CrewHours => Set<CrewHoursRow>();

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        base.OnModelCreating(builder);
        builder.HasDefaultSchema("workspace");
        builder.Entity<RaccordUser>().ToTable("users");
        builder.Entity<IdentityRole<Guid>>().ToTable("roles");
        builder.Entity<IdentityUserClaim<Guid>>().ToTable("user_claims");
        builder.Entity<IdentityUserRole<Guid>>().ToTable("user_roles");
        builder.Entity<IdentityUserLogin<Guid>>().ToTable("user_logins");
        builder.Entity<IdentityUserToken<Guid>>().ToTable("user_tokens");
        builder.Entity<IdentityRoleClaim<Guid>>().ToTable("role_claims");
        builder.Entity<DataProtectionKey>().ToTable("data_protection_keys");
        builder.ApplyConfigurationsFromAssembly(typeof(RaccordDbContext).Assembly);
        ApplySnakeCaseNames(builder);
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
