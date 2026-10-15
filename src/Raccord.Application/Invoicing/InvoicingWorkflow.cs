using System.Globalization;
using System.Security.Cryptography;
using Raccord.Application.Abstractions;
using Raccord.Application.Auditing;
using Raccord.Application.Compliance;
using Raccord.Application.Documents;
using Raccord.Application.Organization;
using Raccord.Application.Projects;
using Raccord.BuildingBlocks.Storage;
using Raccord.Contracts.Invoicing;
using Raccord.Domain.Billing;
using Raccord.Domain.Projects;
using Raccord.Modules.Contacts.Contracts;
using Raccord.SharedKernel.Abstractions;
using Raccord.SharedKernel.Auditing;
using Raccord.SharedKernel.Common;
using Raccord.SharedKernel.Pricing;

namespace Raccord.Application.Invoicing;

/// <summary>Use cases from project billable lines to issued invoices and their documents.</summary>
public sealed class InvoicingWorkflow(
    IInvoiceDraftRepository drafts,
    IIssuedInvoiceRepository invoices,
    IInvoicePaymentRepository payments,
    IInvoiceReminderRepository reminders,
    IInvoiceSeriesRepository series,
    IProjectRepository projects,
    IPartyDirectory parties,
    IIssuerProfileRepository issuers,
    IAuditTrail audit,
    IComplianceRecorder compliance,
    IInvoiceDocumentRenderer renderer,
    IDocumentStore documents,
    IClock clock,
    ICurrentUser actor,
    IUnitOfWork unitOfWork
)
{
    private const string DraftEntity = "invoice-draft";
    private const string InvoiceEntity = "invoice";

    /// <summary>Creates a draft from unbilled lines of a project.</summary>
    public async Task<InvoiceDraftResponse> CreateDraftAsync(
        CreateInvoiceDraftRequest request,
        CancellationToken cancellationToken
    )
    {
        ArgumentNullException.ThrowIfNull(request);
        var project =
            await projects.FindAsync(request.ProjectId, cancellationToken).ConfigureAwait(false)
            ?? throw new BusinessRuleViolationException(
                "projects.not-found",
                $"Project '{request.ProjectId}' does not exist."
            );
        var seriesCode = await ResolveSeriesCodeAsync(request.SeriesCode, cancellationToken)
            .ConfigureAwait(false);

        var draft = InvoiceDraft.CreateFromProject(
            project,
            request.BillableLineIds ?? [],
            seriesCode,
            clock.UtcNow
        );
        drafts.Add(draft);
        Record(
            "invoice-draft.created",
            DraftEntity,
            draft.Id,
            ("projectId", project.Id.ToString()),
            ("lineCount", Text(draft.Lines.Count)),
            ("total", Text(draft.Totals.Total.Amount))
        );
        await unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        return await DescribeAsync(draft, project, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>Creates an invoice draft from manually entered lines without a project.</summary>
    public async Task<InvoiceDraftResponse> CreateStandaloneDraftAsync(
        CreateStandaloneInvoiceRequest request,
        CancellationToken cancellationToken
    )
    {
        ArgumentNullException.ThrowIfNull(request);
        var customer =
            await parties.FindAsync(request.CustomerId, cancellationToken).ConfigureAwait(false)
            ?? throw new BusinessRuleViolationException(
                "customers.not-found",
                $"Customer '{request.CustomerId}' does not exist."
            );
        var seriesCode = await ResolveSeriesCodeAsync(request.SeriesCode, cancellationToken)
            .ConfigureAwait(false);
        var draft = InvoiceDraft.CreateStandalone(
            customer.Id,
            request.OperationDate ?? clock.Today,
            request
                .Lines.Select(line =>
                    (
                        line.Description,
                        line.Quantity,
                        line.UnitPrice,
                        line.DiscountPercent,
                        line.VatRate
                    )
                )
                .ToList(),
            seriesCode,
            clock.UtcNow
        );
        drafts.Add(draft);
        Record(
            "invoice-draft.created",
            DraftEntity,
            draft.Id,
            ("customerId", customer.Id.ToString()),
            ("lineCount", Text(draft.Lines.Count)),
            ("total", Text(draft.Totals.Total.Amount))
        );
        await unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        return await DescribeAsync(draft, null, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>Reads a draft with its totals and current validation issues.</summary>
    public async Task<InvoiceDraftResponse?> GetDraftAsync(
        Guid draftId,
        CancellationToken cancellationToken
    )
    {
        var draft = await drafts.FindAsync(draftId, cancellationToken).ConfigureAwait(false);
        return draft is null
            ? null
            : await DescribeAsync(draft, null, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>Lists open drafts.</summary>
    public async Task<IReadOnlyList<InvoiceDraftSummaryResponse>> ListDraftsAsync(
        CancellationToken cancellationToken
    )
    {
        var open = await drafts.ListOpenAsync(cancellationToken).ConfigureAwait(false);
        var result = new List<InvoiceDraftSummaryResponse>(open.Count);
        foreach (var draft in open)
        {
            var project = draft.ProjectId is { } projectId
                ? await projects.FindAsync(projectId, cancellationToken).ConfigureAwait(false)
                : null;
            var customer = await parties
                .FindAsync(draft.CustomerId, cancellationToken)
                .ConfigureAwait(false);
            var totals = draft.Totals;
            result.Add(
                new(
                    draft.Id,
                    draft.ProjectId,
                    project?.Name ?? string.Empty,
                    customer?.LegalName ?? string.Empty,
                    draft.Lines.Count,
                    totals.Total.Amount,
                    totals.Total.Currency.Code,
                    draft.CreatedAt
                )
            );
        }

        return result;
    }

    /// <summary>Lists eligible project work for the staged-invoicing queue (BR-FIN-QUEUE-ELIGIBILITY).</summary>
    public async Task<IReadOnlyList<InvoiceQueueEntryResponse>> ListQueueAsync(
        DateOnly? from,
        DateOnly? to,
        CancellationToken cancellationToken
    )
    {
        var today = clock.Today;
        var start = from ?? today.AddMonths(-1);
        var end = to ?? today.AddMonths(1);
        var customers = (
            await parties.ListAsync(cancellationToken).ConfigureAwait(false)
        ).ToDictionary(party => party.Id, party => party.LegalName);
        var entries = new List<InvoiceQueueEntryResponse>();
        foreach (var project in await projects.ListAsync(cancellationToken).ConfigureAwait(false))
        {
            if (project.Status is ProjectStatus.Draft or ProjectStatus.Cancelled)
                continue;
            var unbilled = project
                .BillableLines.Where(line => line.State == BillingState.Unbilled)
                .ToList();
            if (unbilled.Count == 0 || project.EndDate < start || project.EndDate > end)
                continue;
            var amount = unbilled.Aggregate(
                Money.Zero(Currency.Eur),
                (sum, line) => sum + line.Pricing.NetAmount
            );
            entries.Add(
                new(
                    project.Id,
                    project.Name,
                    project.CustomerId,
                    customers.GetValueOrDefault(project.CustomerId, string.Empty),
                    project.EndDate,
                    project.EndDate < today,
                    unbilled.Count,
                    amount.Amount,
                    amount.Currency.Code
                )
            );
        }

        return entries
            .OrderBy(entry => entry.DueDate)
            .ThenBy(entry => entry.ProjectName, StringComparer.Ordinal)
            .ToList();
    }

    /// <summary>Removes a line from an open draft and makes its billable line available again.</summary>
    public async Task<InvoiceDraftResponse?> RemoveDraftLineAsync(
        Guid draftId,
        Guid draftLineId,
        CancellationToken cancellationToken
    )
    {
        var draft = await drafts.FindAsync(draftId, cancellationToken).ConfigureAwait(false);
        if (draft is null)
        {
            return null;
        }

        var project = draft.ProjectId is { } projectId
            ? await RequireProjectAsync(projectId, cancellationToken).ConfigureAwait(false)
            : null;
        var billableLineId = draft.RemoveLine(draftLineId);
        if (project is not null && billableLineId is { } sourceId)
            project.ReleaseFromDraft(draft.Id, [sourceId]);
        Record(
            "invoice-draft.line-removed",
            DraftEntity,
            draft.Id,
            ("billableLineId", billableLineId?.ToString() ?? string.Empty)
        );
        await unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        return await DescribeAsync(draft, project, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>Discards an open draft and releases all its lines. Returns false when the draft does not exist.</summary>
    public async Task<bool> DiscardDraftAsync(Guid draftId, CancellationToken cancellationToken)
    {
        var draft = await drafts.FindAsync(draftId, cancellationToken).ConfigureAwait(false);
        if (draft is null)
        {
            return false;
        }

        draft.EnsureOpen();
        var project = draft.ProjectId is { } projectId
            ? await RequireProjectAsync(projectId, cancellationToken).ConfigureAwait(false)
            : null;
        if (project is not null)
            project.ReleaseFromDraft(
                draft.Id,
                [.. draft.Lines.Select(line => line.SourceBillableLineId).OfType<Guid>()]
            );
        drafts.Remove(draft);
        Record(
            "invoice-draft.discarded",
            DraftEntity,
            draft.Id,
            ("projectId", project?.Id.ToString() ?? string.Empty)
        );
        await unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        return true;
    }

    /// <summary>
    /// Validates, numbers and issues a draft; stores the immutable representation, the PDF, the compliance record
    /// and the audit events. Either all of it is committed or none of it.
    /// </summary>
    public async Task<IssuedInvoiceResponse?> FinalizeAsync(
        Guid draftId,
        FinalizeInvoiceDraftRequest request,
        CancellationToken cancellationToken
    )
    {
        ArgumentNullException.ThrowIfNull(request);
        var draft = await drafts.FindAsync(draftId, cancellationToken).ConfigureAwait(false);
        if (draft is null)
        {
            return null;
        }

        draft.EnsureOpen();
        var issueDate = request.IssueDate ?? clock.Today;
        var project = draft.ProjectId is { } projectId
            ? await RequireProjectAsync(projectId, cancellationToken).ConfigureAwait(false)
            : null;
        var customer = await parties
            .FindAsync(draft.CustomerId, cancellationToken)
            .ConfigureAwait(false);
        var issuer = (
            await issuers.FindAsync(cancellationToken).ConfigureAwait(false)
        )?.ToLegalParty();
        var recipient = customer?.ToLegalParty();
        var sequence = await series
            .GetOrStartSequenceAsync(draft.SeriesCode, issueDate.Year, cancellationToken)
            .ConfigureAwait(false);

        var issues = draft.Validate(issuer, recipient, issueDate, sequence);
        if (issues.Count > 0)
        {
            throw new InvoiceValidationException(issues);
        }

        var invoice = draft.Finalize(issuer!, recipient!, sequence, issueDate, clock.UtcNow);
        project?.MarkInvoiced(draft.Id, invoice.Id);
        var representation = IssuedInvoiceRepresentation.From(invoice);
        invoices.Add(invoice, representation);
        compliance.RegisterIssuedInvoice(invoice, representation);
        Record(
            "invoice.issued",
            InvoiceEntity,
            invoice.Id,
            ("number", invoice.Number.Value),
            ("draftId", draft.Id.ToString()),
            ("issueDate", invoice.IssueDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)),
            ("total", Text(invoice.Totals.Total.Amount)),
            ("contentHash", representation.ContentHash)
        );

        await StoreDocumentAsync(invoice, "es", "invoice.document.stored", cancellationToken)
            .ConfigureAwait(false);
        await unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        return invoice.ToResponse(representation.ContentHash);
    }

    /// <summary>Lists issued invoices.</summary>
    public async Task<IReadOnlyList<IssuedInvoiceSummaryResponse>> ListInvoicesAsync(
        CancellationToken cancellationToken
    )
    {
        var items = await invoices.ListAsync(cancellationToken).ConfigureAwait(false);
        return
        [
            .. items.Select(item => new IssuedInvoiceSummaryResponse(
                item.Id,
                item.Number,
                item.IssueDate,
                item.RecipientName,
                item.Total,
                item.Currency,
                item.ProjectId
            )),
        ];
    }

    /// <summary>Reads an issued invoice after verifying its integrity.</summary>
    public async Task<IssuedInvoiceResponse?> GetInvoiceAsync(
        Guid invoiceId,
        CancellationToken cancellationToken
    )
    {
        var record = await invoices.FindAsync(invoiceId, cancellationToken).ConfigureAwait(false);
        return record?.Invoice.ToResponse(record.ContentHash);
    }

    /// <summary>Records a validated payment and recalculates the outstanding balance.</summary>
    public async Task<InvoicePaymentSummaryResponse?> RecordPaymentAsync(
        Guid invoiceId,
        RecordInvoicePaymentRequest request,
        CancellationToken cancellationToken
    )
    {
        ArgumentNullException.ThrowIfNull(request);
        var invoice = await invoices.FindAsync(invoiceId, cancellationToken).ConfigureAwait(false);
        if (invoice is null)
            return null;
        var existing = await payments.ListAsync(invoiceId, cancellationToken).ConfigureAwait(false);
        var outstanding =
            invoice.Invoice.Totals.Total.Amount - existing.Sum(payment => payment.Amount);
        if (request.Amount > outstanding)
            throw new BusinessRuleViolationException(
                "payment.exceeds-outstanding",
                "The payment cannot exceed the invoice outstanding amount."
            );

        var payment = InvoicePayment.Create(
            invoiceId,
            request.Date,
            request.Method,
            request.Amount,
            request.Description,
            clock.UtcNow
        );
        payments.Add(payment);
        Record(
            "invoice.payment-recorded",
            InvoiceEntity,
            invoiceId,
            ("paymentId", payment.Id.ToString()),
            ("amount", Text(payment.Amount))
        );
        await unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        return await DescribePaymentsAsync(invoice.Invoice, [.. existing, payment])
            .ConfigureAwait(false);
    }

    /// <summary>Reads payment history and the derived payment status.</summary>
    public async Task<InvoicePaymentSummaryResponse?> ListPaymentsAsync(
        Guid invoiceId,
        CancellationToken cancellationToken
    )
    {
        var invoice = await invoices.FindAsync(invoiceId, cancellationToken).ConfigureAwait(false);
        if (invoice is null)
            return null;
        var existing = await payments.ListAsync(invoiceId, cancellationToken).ConfigureAwait(false);
        return await DescribePaymentsAsync(invoice.Invoice, existing).ConfigureAwait(false);
    }

    /// <summary>Lists unpaid invoices whose current due date has passed.</summary>
    public async Task<IReadOnlyList<OverdueInvoiceResponse>> ListOverdueInvoicesAsync(
        CancellationToken cancellationToken
    )
    {
        var overdue = new List<OverdueInvoiceResponse>();
        foreach (var item in await invoices.ListAsync(cancellationToken).ConfigureAwait(false))
        {
            var invoice = await invoices
                .FindAsync(item.Id, cancellationToken)
                .ConfigureAwait(false);
            if (invoice is null)
                continue;
            var paymentsForInvoice = await payments
                .ListAsync(item.Id, cancellationToken)
                .ConfigureAwait(false);
            var outstanding =
                invoice.Invoice.Totals.Total.Amount
                - paymentsForInvoice.Sum(payment => payment.Amount);
            if (outstanding <= 0m)
                continue;
            var invoiceReminders = await reminders
                .ListAsync(item.Id, cancellationToken)
                .ConfigureAwait(false);
            var lastReminder = invoiceReminders.Count > 0 ? invoiceReminders[^1] : null;
            var dueDate = lastReminder?.DueDate ?? DefaultDueDate(invoice.Invoice.IssueDate);
            if (dueDate >= clock.Today)
                continue;
            overdue.Add(
                new(
                    item.Id,
                    item.Number,
                    invoice.Invoice.Source.CustomerId,
                    invoice.Invoice.Recipient.LegalName,
                    dueDate,
                    outstanding,
                    invoiceReminders.Count,
                    lastReminder?.CreatedOn
                )
            );
        }

        return overdue
            .OrderBy(entry => entry.DueDate)
            .ThenBy(entry => entry.InvoiceNumber, StringComparer.Ordinal)
            .ToArray();
    }

    /// <summary>Generates the next manual reminder for an overdue invoice.</summary>
    public async Task<InvoiceReminderResponse?> CreateReminderAsync(
        Guid invoiceId,
        CreateInvoiceReminderRequest request,
        CancellationToken cancellationToken
    )
    {
        ArgumentNullException.ThrowIfNull(request);
        var invoice = await invoices.FindAsync(invoiceId, cancellationToken).ConfigureAwait(false);
        if (invoice is null)
            return null;
        var paymentsForInvoice = await payments
            .ListAsync(invoiceId, cancellationToken)
            .ConfigureAwait(false);
        var outstanding =
            invoice.Invoice.Totals.Total.Amount - paymentsForInvoice.Sum(payment => payment.Amount);
        if (outstanding <= 0m)
            throw new BusinessRuleViolationException(
                "invoice.not-overdue",
                "A paid invoice cannot receive a reminder."
            );

        var existing = await reminders
            .ListAsync(invoiceId, cancellationToken)
            .ConfigureAwait(false);
        var lastReminder = existing.Count > 0 ? existing[^1] : null;
        var previousDueDate = lastReminder?.DueDate ?? DefaultDueDate(invoice.Invoice.IssueDate);
        if (previousDueDate >= clock.Today)
            throw new BusinessRuleViolationException(
                "invoice.not-overdue",
                "A reminder can only be generated after the due date."
            );

        var reminder = InvoiceReminder.Create(
            invoiceId,
            existing.Count + 1,
            clock.Today,
            request.NewDueDate ?? previousDueDate,
            request.Template,
            request.Letterhead
        );
        reminders.Add(reminder);
        Record(
            "invoice.payment-reminder-generated",
            InvoiceEntity,
            invoiceId,
            ("reminderId", reminder.Id.ToString()),
            ("number", Text(reminder.Number)),
            ("dueDate", reminder.DueDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture))
        );
        await unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        return ToReminderResponse(reminder);
    }

    /// <summary>Reads the manual reminder history of an invoice.</summary>
    public async Task<IReadOnlyList<InvoiceReminderResponse>?> ListRemindersAsync(
        Guid invoiceId,
        CancellationToken cancellationToken
    )
    {
        if (await invoices.FindAsync(invoiceId, cancellationToken).ConfigureAwait(false) is null)
            return null;
        return (await reminders.ListAsync(invoiceId, cancellationToken).ConfigureAwait(false))
            .Select(ToReminderResponse)
            .ToArray();
    }

    /// <summary>Duplicates an issued invoice into a new draft using current source data.</summary>
    public async Task<InvoiceDraftResponse?> DuplicateInvoiceAsync(
        Guid invoiceId,
        CancellationToken cancellationToken
    )
    {
        var original = await invoices.FindAsync(invoiceId, cancellationToken).ConfigureAwait(false);
        if (original is null)
            return null;

        var sourceDraft =
            await drafts
                .FindAsync(original.Invoice.Source.DraftId, cancellationToken)
                .ConfigureAwait(false)
            ?? throw new BusinessRuleViolationException(
                "invoice.duplicate-source-missing",
                "The invoice draft source no longer exists."
            );
        InvoiceDraft draft;
        Project? project = null;
        if (original.Invoice.Source.ProjectId is { } projectId)
        {
            project = await RequireProjectAsync(projectId, cancellationToken).ConfigureAwait(false);
            draft = InvoiceDraft.DuplicateFromProject(
                project,
                sourceDraft.Lines.Select(line => line.SourceBillableLineId).OfType<Guid>().ToList(),
                sourceDraft.SeriesCode,
                clock.UtcNow
            );
        }
        else
        {
            draft = InvoiceDraft.CreateStandalone(
                original.Invoice.Source.CustomerId,
                sourceDraft.OperationDate,
                sourceDraft
                    .Lines.Select(line =>
                        (
                            line.Description,
                            line.Quantity,
                            line.UnitPrice,
                            line.DiscountPercent,
                            line.VatPercentage
                        )
                    )
                    .ToList(),
                sourceDraft.SeriesCode,
                clock.UtcNow
            );
        }

        drafts.Add(draft);
        Record(
            "invoice-draft.duplicated",
            DraftEntity,
            draft.Id,
            ("sourceInvoiceId", invoiceId.ToString()),
            ("projectId", project?.Id.ToString() ?? string.Empty)
        );
        await unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        return await DescribeAsync(draft, project, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>Starts a rectifying draft from an immutable issued invoice.</summary>
    public async Task<InvoiceDraftResponse?> CreateRectificationDraftAsync(
        Guid invoiceId,
        RectifyInvoiceRequest request,
        CancellationToken cancellationToken
    )
    {
        ArgumentNullException.ThrowIfNull(request);
        var original = await invoices.FindAsync(invoiceId, cancellationToken).ConfigureAwait(false);
        if (original is null)
            return null;

        if (!Enum.TryParse<RectificationKind>(request.Kind, true, out var kind))
            throw new BusinessRuleViolationException(
                "rectification.kind-invalid",
                "Rectification kind must be differences or substitution."
            );

        var draft = InvoiceDraft.CreateRectification(
            original.Invoice,
            kind,
            request.Reason,
            clock.UtcNow
        );
        drafts.Add(draft);
        Record(
            "invoice-rectification-draft.created",
            DraftEntity,
            draft.Id,
            ("originalInvoiceId", invoiceId.ToString()),
            ("kind", kind.ToString()),
            ("reason", request.Reason.Trim())
        );
        await unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        return await DescribeAsync(draft, null, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>Returns the default Spanish invoice document, regenerating it when missing.</summary>
    public async Task<InvoiceDocument?> GetDocumentAsync(
        Guid invoiceId,
        CancellationToken cancellationToken
    ) => await GetDocumentAsync(invoiceId, "es", cancellationToken).ConfigureAwait(false);

    /// <summary>Returns the invoice document in the requested presentation language.</summary>
    public async Task<InvoiceDocument?> GetDocumentAsync(
        Guid invoiceId,
        string language,
        CancellationToken cancellationToken
    )
    {
        language = NormalizeLanguage(language);
        var record = await invoices.FindAsync(invoiceId, cancellationToken).ConfigureAwait(false);
        if (record is null)
        {
            return null;
        }

        var invoice = record.Invoice;
        var content = await documents
            .ReadAsync(DocumentKey(invoice, language), cancellationToken)
            .ConfigureAwait(false);
        if (content is null)
        {
            content = await StoreDocumentAsync(
                    invoice,
                    language,
                    "invoice.document.regenerated",
                    cancellationToken
                )
                .ConfigureAwait(false);
            await unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }

        return new InvoiceDocument(
            $"{invoice.Number.Value}.{renderer.FileExtension}",
            renderer.ContentType,
            content
        );
    }

    private async Task<byte[]> StoreDocumentAsync(
        IssuedInvoice invoice,
        string language,
        string action,
        CancellationToken cancellationToken
    )
    {
        var content = renderer.Render(invoice, language);
        var key = DocumentKey(invoice, language);
        await documents
            .SaveAsync(key, content, renderer.ContentType, cancellationToken)
            .ConfigureAwait(false);
        Record(
            action,
            InvoiceEntity,
            invoice.Id,
            ("documentKey", key),
            ("sha256", Convert.ToHexStringLower(SHA256.HashData(content))),
            ("bytes", Text(content.Length))
        );
        return content;
    }

    private string DocumentKey(IssuedInvoice invoice, string language) =>
        $"invoices/{invoice.Id:N}/{invoice.Number.Value}-{language}.{renderer.FileExtension}";

    private static string NormalizeLanguage(string language) =>
        string.Equals(language, "en", StringComparison.OrdinalIgnoreCase) ? "en" : "es";

    private async Task<InvoiceDraftResponse> DescribeAsync(
        InvoiceDraft draft,
        Project? project,
        CancellationToken cancellationToken
    )
    {
        PartySnapshot? customer = await parties
            .FindAsync(draft.CustomerId, cancellationToken)
            .ConfigureAwait(false);
        IReadOnlyList<ValidationIssue> issues = [];
        if (draft.Status == DraftStatus.Open)
        {
            var today = clock.Today;
            var sequence =
                await series
                    .FindSequenceAsync(draft.SeriesCode, today.Year, cancellationToken)
                    .ConfigureAwait(false)
                ?? InvoiceNumberSequence.Start(draft.SeriesCode, today.Year);
            var issuer = (
                await issuers.FindAsync(cancellationToken).ConfigureAwait(false)
            )?.ToLegalParty();
            issues = draft.Validate(issuer, customer?.ToLegalParty(), today, sequence);
        }

        return new InvoiceDraftResponse(
            draft.Id,
            draft.Status == DraftStatus.Open ? "open" : "issued",
            project?.Id,
            project?.Name,
            draft.CustomerId,
            customer?.LegalName ?? string.Empty,
            draft.SeriesCode,
            draft.OperationDate,
            draft.CreatedAt,
            [.. draft.Lines.Select(line => line.ToResponse())],
            draft.Totals.ToResponse(),
            [.. issues.Select(issue => issue.ToResponse())],
            draft.IssuedInvoiceId
        );
    }

    private async Task<string> ResolveSeriesCodeAsync(
        string? requested,
        CancellationToken cancellationToken
    )
    {
        if (!string.IsNullOrWhiteSpace(requested))
        {
            return (
                    await series.FindAsync(requested, cancellationToken).ConfigureAwait(false)
                )?.Code
                ?? throw new BusinessRuleViolationException(
                    "numbering.series-not-found",
                    $"Invoice series '{requested}' does not exist."
                );
        }

        var all = await series.ListAsync(cancellationToken).ConfigureAwait(false);
        return all.FirstOrDefault(candidate => candidate.IsDefault)?.Code
            ?? throw new BusinessRuleViolationException(
                "numbering.default-series-missing",
                "No default invoice series is configured."
            );
    }

    private static Task<InvoicePaymentSummaryResponse> DescribePaymentsAsync(
        IssuedInvoice invoice,
        IReadOnlyList<InvoicePayment> payments
    )
    {
        var totalPaid = payments.Sum(payment => payment.Amount);
        var outstanding = invoice.Totals.Total.Amount - totalPaid;
        return Task.FromResult(
            new InvoicePaymentSummaryResponse(
                [
                    .. payments.Select(payment => new InvoicePaymentResponse(
                        payment.Id,
                        payment.Date,
                        payment.Method,
                        payment.Amount,
                        payment.Description,
                        payment.RecordedAt
                    )),
                ],
                totalPaid,
                outstanding,
                outstanding == 0m ? "paid"
                    : totalPaid == 0m ? "unpaid"
                    : "partially-paid"
            )
        );
    }

    private static DateOnly DefaultDueDate(DateOnly issueDate) => issueDate.AddDays(30);

    private static InvoiceReminderResponse ToReminderResponse(InvoiceReminder reminder) =>
        new(
            reminder.Id,
            reminder.InvoiceId,
            reminder.Number,
            reminder.CreatedOn,
            reminder.DueDate,
            reminder.Template,
            reminder.Letterhead
        );

    private async Task<Project> RequireProjectAsync(
        Guid projectId,
        CancellationToken cancellationToken
    ) =>
        await projects.FindAsync(projectId, cancellationToken).ConfigureAwait(false)
        ?? throw new BusinessRuleViolationException(
            "projects.not-found",
            $"Project '{projectId}' does not exist."
        );

    private void Record(
        string action,
        string entityType,
        Guid entityId,
        params (string Key, string Value)[] details
    ) =>
        audit.Append(
            new AuditEntry(
                clock.UtcNow,
                actor.Name,
                action,
                entityType,
                entityId,
                details.ToDictionary(
                    detail => detail.Key,
                    detail => detail.Value,
                    StringComparer.Ordinal
                )
            )
        );

    private static string Text(decimal value) => value.ToString(CultureInfo.InvariantCulture);

    private static string Text(int value) => value.ToString(CultureInfo.InvariantCulture);
}
