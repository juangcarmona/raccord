using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Raccord.Domain.Billing;

namespace Raccord.Infrastructure.Persistence.Configurations;

internal sealed class InvoiceReminderConfiguration : IEntityTypeConfiguration<InvoiceReminder>
{
    public void Configure(EntityTypeBuilder<InvoiceReminder> builder)
    {
        builder.ToTable("invoice_reminders");
        builder.HasKey(reminder => reminder.Id);
        builder.Property(reminder => reminder.Template).HasMaxLength(200);
        builder.Property(reminder => reminder.Letterhead).HasMaxLength(200);
        builder.HasIndex(reminder => new { reminder.InvoiceId, reminder.Number }).IsUnique();
    }
}
