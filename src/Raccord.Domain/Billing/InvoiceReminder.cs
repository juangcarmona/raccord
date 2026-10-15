namespace Raccord.Domain.Billing;

/// <summary>A manually generated reminder for an overdue invoice.</summary>
public sealed class InvoiceReminder
{
    private InvoiceReminder() { }

    private InvoiceReminder(
        Guid id,
        Guid invoiceId,
        int number,
        DateOnly createdOn,
        DateOnly dueDate,
        string? template,
        string? letterhead
    )
    {
        Id = id;
        InvoiceId = invoiceId;
        Number = number;
        CreatedOn = createdOn;
        DueDate = dueDate;
        Template = template;
        Letterhead = letterhead;
    }

    /// <summary>Reminder identifier.</summary>
    public Guid Id { get; private set; }

    /// <summary>Issued invoice receiving the reminder.</summary>
    public Guid InvoiceId { get; private set; }

    /// <summary>One-based reminder number for the invoice.</summary>
    public int Number { get; private set; }

    /// <summary>Date the reminder was generated.</summary>
    public DateOnly CreatedOn { get; private set; }

    /// <summary>New payment deadline recorded by the reminder.</summary>
    public DateOnly DueDate { get; private set; }

    /// <summary>Optional reminder template identifier.</summary>
    public string? Template { get; private set; }

    /// <summary>Optional letterhead identifier.</summary>
    public string? Letterhead { get; private set; }

    /// <summary>Creates the next reminder in an invoice's manual reminder history.</summary>
    public static InvoiceReminder Create(
        Guid invoiceId,
        int number,
        DateOnly createdOn,
        DateOnly dueDate,
        string? template,
        string? letterhead
    )
    {
        if (invoiceId == Guid.Empty)
            throw new ArgumentException("Invoice is required.", nameof(invoiceId));
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(number);
        if (dueDate < createdOn)
            throw new ArgumentException(
                "Reminder due date cannot be before its creation date.",
                nameof(dueDate)
            );

        return new(
            Guid.CreateVersion7(),
            invoiceId,
            number,
            createdOn,
            dueDate,
            string.IsNullOrWhiteSpace(template) ? null : template.Trim(),
            string.IsNullOrWhiteSpace(letterhead) ? null : letterhead.Trim()
        );
    }
}
