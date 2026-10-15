using System.Globalization;
using System.Text;
using Raccord.Application.Invoicing;
using Raccord.Contracts.Finance;
using Raccord.Domain.Billing;
using Raccord.SharedKernel.Abstractions;
using Raccord.SharedKernel.Common;

namespace Raccord.Application.Finance;

/// <summary>Produces live finance views and finalized-invoice exports.</summary>
public sealed class FinanceReportingWorkflow(
    IIssuedInvoiceRepository invoices,
    IInvoicePaymentRepository payments,
    IClock clock
)
{
    /// <summary>Summarizes finalized invoices and their payments in a date range.</summary>
    public async Task<FinanceSummaryResponse> SummarizeAsync(
        DateOnly from,
        DateOnly to,
        CancellationToken cancellationToken
    )
    {
        var rows = await LoadAsync(from, to, cancellationToken).ConfigureAwait(false);
        var paid = 0m;
        var outstanding = 0m;
        var overdue = 0;
        foreach (var row in rows)
        {
            var totalPaid = (
                await payments.ListAsync(row.Invoice.Id, cancellationToken).ConfigureAwait(false)
            ).Sum(payment => payment.Amount);
            paid += totalPaid;
            outstanding += row.Invoice.Totals.Total.Amount - totalPaid;
            if (
                row.Invoice.Totals.Total.Amount > totalPaid
                && row.Invoice.IssueDate.AddDays(30) < clock.Today
            )
                overdue++;
        }

        return new(
            from,
            to,
            rows.Count,
            rows.Sum(row => row.Invoice.Totals.Total.Amount),
            rows.Sum(row => row.Invoice.Totals.VatAmount.Amount),
            paid,
            outstanding,
            overdue
        );
    }

    /// <summary>Exports finalized invoices, VAT groups and payments as UTF-8 CSV.</summary>
    public async Task<byte[]> ExportAccountingCsvAsync(
        DateOnly from,
        DateOnly to,
        CancellationToken cancellationToken
    )
    {
        var rows = await LoadAsync(from, to, cancellationToken).ConfigureAwait(false);
        if (rows.Count == 0)
            throw new BusinessRuleViolationException(
                "accounting-export.empty",
                "The selected period contains no finalized invoices."
            );

        var csv = new StringBuilder(
            "invoice_number,issue_date,customer,kind,amount,currency,vat_rate,payment_method\n"
        );
        foreach (var row in rows)
        {
            var invoice = row.Invoice;
            await AppendInvoiceRows(csv, invoice, cancellationToken).ConfigureAwait(false);
        }

        return Encoding.UTF8.GetBytes(csv.ToString());
    }

    private async Task AppendInvoiceRows(
        StringBuilder csv,
        IssuedInvoice invoice,
        CancellationToken cancellationToken
    )
    {
        foreach (var vat in invoice.Totals.VatBreakdown)
            Append(
                csv,
                invoice.Number.Value,
                invoice.IssueDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                invoice.Recipient.LegalName,
                "vat",
                vat.VatAmount.Amount.ToString("0.00", CultureInfo.InvariantCulture),
                invoice.Currency.Code,
                vat.Rate.Percentage.ToString("0.##", CultureInfo.InvariantCulture),
                ""
            );
        Append(
            csv,
            invoice.Number.Value,
            invoice.IssueDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
            invoice.Recipient.LegalName,
            "invoice",
            invoice.Totals.Total.Amount.ToString("0.00", CultureInfo.InvariantCulture),
            invoice.Currency.Code,
            "",
            ""
        );
        foreach (
            var payment in await payments
                .ListAsync(invoice.Id, cancellationToken)
                .ConfigureAwait(false)
        )
            Append(
                csv,
                invoice.Number.Value,
                payment.Date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                invoice.Recipient.LegalName,
                "payment",
                payment.Amount.ToString("0.00", CultureInfo.InvariantCulture),
                invoice.Currency.Code,
                "",
                payment.Method
            );
    }

    private async Task<List<IssuedInvoiceRecord>> LoadAsync(
        DateOnly from,
        DateOnly to,
        CancellationToken cancellationToken
    )
    {
        if (from > to)
            throw new BusinessRuleViolationException(
                "finance.invalid-period",
                "The start date must not be after the end date."
            );

        var rows = new List<IssuedInvoiceRecord>();
        foreach (var item in await invoices.ListAsync(cancellationToken).ConfigureAwait(false))
        {
            if (item.IssueDate < from || item.IssueDate > to)
                continue;
            if (
                await invoices.FindAsync(item.Id, cancellationToken).ConfigureAwait(false) is
                { } invoice
            )
                rows.Add(invoice);
        }

        return rows;
    }

    private static void Append(StringBuilder csv, params string[] values) =>
        csv.AppendLine(
            string.Join(
                ',',
                values.Select(value =>
                    $"\"{value.Replace("\"", "\"\"", StringComparison.Ordinal)}\""
                )
            )
        );
}
