using Raccord.SharedKernel.Common;

namespace Raccord.Domain.Billing;

/// <summary>A payment received against an issued invoice.</summary>
public sealed class InvoicePayment
{
    private InvoicePayment() { }

    private InvoicePayment(
        Guid invoiceId,
        DateOnly date,
        string method,
        decimal amount,
        string? description,
        DateTimeOffset recordedAt
    )
    {
        Id = Guid.CreateVersion7();
        InvoiceId = invoiceId;
        Date = date;
        Method = method.Trim();
        Amount = amount;
        Description = description?.Trim();
        RecordedAt = recordedAt;
    }

    /// <summary>Payment identifier.</summary>
    public Guid Id { get; private set; }

    /// <summary>Issued invoice receiving the payment.</summary>
    public Guid InvoiceId { get; private set; }

    /// <summary>Date the money was received.</summary>
    public DateOnly Date { get; private set; }

    /// <summary>Payment method chosen by the operator.</summary>
    public string Method { get; private set; } = string.Empty;

    /// <summary>Positive amount received.</summary>
    public decimal Amount { get; private set; }

    /// <summary>Optional operator description.</summary>
    public string? Description { get; private set; }

    /// <summary>UTC time the record was created.</summary>
    public DateTimeOffset RecordedAt { get; private set; }

    /// <summary>Creates a validated payment for an issued invoice.</summary>
    public static InvoicePayment Create(
        Guid invoiceId,
        DateOnly date,
        string method,
        decimal amount,
        string? description,
        DateTimeOffset recordedAt
    )
    {
        if (date == default)
            throw new BusinessRuleViolationException(
                "payment.date-required",
                "A payment date is required."
            );
        if (string.IsNullOrWhiteSpace(method))
            throw new BusinessRuleViolationException(
                "payment.method-required",
                "A payment method is required."
            );
        if (amount <= 0m)
            throw new BusinessRuleViolationException(
                "payment.amount-positive",
                "Payment amount must be positive."
            );
        return new InvoicePayment(invoiceId, date, method, amount, description, recordedAt);
    }
}
