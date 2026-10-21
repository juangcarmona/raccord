using Raccord.Contracts.Projects;
using Raccord.Domain.Projects;
using Raccord.Modules.Contacts.Contracts;
using Raccord.Modules.Invoicing.Contracts;
using Raccord.Modules.Projects.Contracts;
using Raccord.SharedKernel.Common;
using Raccord.SharedKernel.Pricing;

namespace Raccord.Modules.Projects;

/// <summary>Use cases for projects and their billable work.</summary>
public sealed class ProjectWorkspace(
    IBillableWork projects,
    IPartyDirectory parties,
    IEquipmentStock equipment,
    IProjectsUnitOfWork unitOfWork,
    IVatSchemePricing vatPricing
)
{
    /// <summary>Creates a project for an existing customer.</summary>
    public async Task<ProjectResponse> CreateAsync(
        CreateProjectRequest request,
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
        var project = Project.Create(
            request.Name ?? string.Empty,
            customer.Id,
            request.StartDate,
            request.EndDate,
            request.ProjectTypeId,
            request.TeamId
        );
        projects.Add(project);
        await unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        return ToResponse(project, customer.LegalName);
    }

    /// <summary>Lists projects with their unbilled work.</summary>
    public async Task<IReadOnlyList<ProjectSummaryResponse>> ListAsync(
        CancellationToken cancellationToken
    )
    {
        var all = await projects.ListAsync(cancellationToken).ConfigureAwait(false);
        var names = (await parties.ListAsync(cancellationToken).ConfigureAwait(false)).ToDictionary(
            customer => customer.Id,
            customer => customer.LegalName
        );
        return
        [
            .. all.Select(project =>
            {
                var unbilled = project
                    .BillableLines.Where(line => line.State == BillingState.Unbilled)
                    .ToList();
                var amount = unbilled.Aggregate(
                    Money.Zero(Currency.Eur),
                    (sum, line) => sum + line.Pricing.NetAmount
                );
                return new ProjectSummaryResponse(
                    project.Id,
                    project.Name,
                    project.CustomerId,
                    names.GetValueOrDefault(project.CustomerId, string.Empty),
                    project.StartDate,
                    project.EndDate,
                    unbilled.Count,
                    amount.Amount,
                    amount.Currency.Code,
                    project.ProjectTypeId,
                    project.TeamId,
                    project.RefundableDepositAmount,
                    (Raccord.Contracts.Projects.DepositStatus)project.DepositStatus
                );
            }),
        ];
    }

    /// <summary>Reads a project with its billable lines.</summary>
    public async Task<ProjectResponse?> GetAsync(Guid id, CancellationToken cancellationToken)
    {
        var project = await projects.FindAsync(id, cancellationToken).ConfigureAwait(false);
        if (project is null)
        {
            return null;
        }

        var customer = await parties
            .FindAsync(project.CustomerId, cancellationToken)
            .ConfigureAwait(false);
        return ToResponse(project, customer?.LegalName ?? string.Empty);
    }

    /// <summary>Configures a project's fixed refundable deposit.</summary>
    public async Task<ProjectResponse?> SetDepositAsync(
        Guid projectId,
        SetRefundableDepositRequest request,
        CancellationToken cancellationToken
    )
    {
        ArgumentNullException.ThrowIfNull(request);
        var project = await projects.FindAsync(projectId, cancellationToken).ConfigureAwait(false);
        if (project is null)
            return null;
        project.SetRefundableDeposit(request.Amount);
        await unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        var customer = await parties
            .FindAsync(project.CustomerId, cancellationToken)
            .ConfigureAwait(false);
        return ToResponse(project, customer?.LegalName ?? string.Empty);
    }

    /// <summary>Moves a configured deposit through receipt and return.</summary>
    public async Task<ProjectResponse?> SetDepositStatusAsync(
        Guid projectId,
        SetDepositStatusRequest request,
        CancellationToken cancellationToken
    )
    {
        ArgumentNullException.ThrowIfNull(request);
        var project = await projects.FindAsync(projectId, cancellationToken).ConfigureAwait(false);
        if (project is null)
            return null;
        project.SetDepositStatus((global::Raccord.Domain.Projects.DepositStatus)request.Status);
        await unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        var customer = await parties
            .FindAsync(project.CustomerId, cancellationToken)
            .ConfigureAwait(false);
        return ToResponse(project, customer?.LegalName ?? string.Empty);
    }

    /// <summary>Records billable work on a project.</summary>
    public async Task<BillableLineResponse?> AddBillableLineAsync(
        Guid projectId,
        AddBillableLineRequest request,
        CancellationToken cancellationToken
    )
    {
        ArgumentNullException.ThrowIfNull(request);
        var project = await projects.FindAsync(projectId, cancellationToken).ConfigureAwait(false);
        if (project is null)
        {
            return null;
        }

        var kind = Enum.GetValues<BillableLineKind>()
            .FirstOrDefault(candidate =>
                string.Equals(
                    candidate.ToString(),
                    request.Kind,
                    StringComparison.OrdinalIgnoreCase
                )
            );
        if (kind == default)
        {
            throw new BusinessRuleViolationException(
                "projects.billable-line-kind-invalid",
                $"Unknown billable line kind '{request.Kind}'."
            );
        }

        var vatSelection = await vatPricing
            .ResolveRateAsync(request.VatClassId, request.VatSchemeId, cancellationToken)
            .ConfigureAwait(false);
        var pricing = LinePricing.Create(
            request.Quantity,
            request.UnitPrice,
            request.DiscountPercent,
            VatRate.Of(vatSelection.Percentage)
        );
        var line = project.AddBillableLine(
            kind,
            request.Description ?? string.Empty,
            pricing,
            vatSelection.VatClassId,
            vatSelection.VatSchemeId
        );
        vatPricing.MarkUsed(vatSelection.VatSchemeId);
        await unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        return ToResponse(line);
    }

    /// <summary>Adds equipment to a project's warehouse packing list.</summary>
    public async Task<PackingLineResponse?> AddPackingLineAsync(
        Guid projectId,
        PlanEquipmentRequest request,
        CancellationToken cancellationToken
    )
    {
        ArgumentNullException.ThrowIfNull(request);
        var project = await projects.FindAsync(projectId, cancellationToken).ConfigureAwait(false);
        if (project is null)
            return null;
        var line = project.AddPackingLine(
            request.EquipmentId,
            request.Quantity,
            ParsePackingRole(request.Role)
        );
        await unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        return ToResponse(line);
    }

    /// <summary>Adjusts a planned packing line before warehouse processing begins.</summary>
    public async Task<PackingLineResponse?> AdjustPackingLineAsync(
        Guid projectId,
        Guid lineId,
        AdjustPackingLineRequest request,
        CancellationToken cancellationToken
    )
    {
        ArgumentNullException.ThrowIfNull(request);
        var project = await projects.FindAsync(projectId, cancellationToken).ConfigureAwait(false);
        if (project is null)
            return null;
        var line = project.AdjustPackingLine(
            lineId,
            request.Quantity,
            ParsePackingRole(request.Role)
        );
        await unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        return ToResponse(line);
    }

    /// <summary>Records a warehouse exception in the packing trace.</summary>
    public async Task<PackingLineResponse?> RecordExceptionAsync(
        Guid projectId,
        Guid lineId,
        RecordPackingExceptionRequest request,
        CancellationToken cancellationToken
    )
    {
        ArgumentNullException.ThrowIfNull(request);
        var project = await projects.FindAsync(projectId, cancellationToken).ConfigureAwait(false);
        if (project is null)
            return null;
        var line = project.PackingLines.FirstOrDefault(candidate => candidate.Id == lineId);
        if (line is null)
            return null;
        line.RecordException(
            request.Kind,
            request.Description,
            request.Actor,
            DateTimeOffset.UtcNow
        );
        await unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        return ToResponse(line);
    }

    /// <summary>Reviews a cross-docking or sale-return disposition.</summary>
    public async Task<PackingLineResponse?> ReviewDispositionAsync(
        Guid projectId,
        Guid lineId,
        ReviewPackingDispositionRequest request,
        CancellationToken cancellationToken
    )
    {
        ArgumentNullException.ThrowIfNull(request);
        var project = await projects.FindAsync(projectId, cancellationToken).ConfigureAwait(false);
        if (project is null)
            return null;
        var line = project.PackingLines.FirstOrDefault(candidate => candidate.Id == lineId);
        if (line is null)
            return null;
        var dispositionName = request.Disposition.Replace(
            "-",
            string.Empty,
            StringComparison.Ordinal
        );
        if (!Enum.TryParse<PackingLineDisposition>(dispositionName, true, out var disposition))
            throw new BusinessRuleViolationException(
                "projects.disposition-invalid",
                $"Unknown packing disposition '{request.Disposition}'."
            );
        line.SetDisposition(disposition, request.Actor);
        await unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        return ToResponse(line);
    }

    /// <summary>Marks a project packing line as prepared.</summary>
    public Task<PackingLineResponse?> PackAsync(
        Guid projectId,
        Guid lineId,
        PackLineRequest request,
        CancellationToken cancellationToken
    ) =>
        TransitionPackingAsync(
            projectId,
            lineId,
            request.Actor,
            null,
            PackingTransition.Pack,
            static (project, id, actor, at) => project.Pack(id, actor, at),
            cancellationToken
        );

    /// <summary>Marks a prepared project packing line as dispatched.</summary>
    public Task<PackingLineResponse?> BookOutAsync(
        Guid projectId,
        Guid lineId,
        BookOutLineRequest request,
        CancellationToken cancellationToken
    ) =>
        TransitionPackingAsync(
            projectId,
            lineId,
            request.Actor,
            request.ScanCode,
            PackingTransition.BookOut,
            static (project, id, actor, at) => project.BookOut(id, actor, at),
            cancellationToken
        );

    /// <summary>Marks a dispatched project packing line as received.</summary>
    public Task<PackingLineResponse?> ReceiveAsync(
        Guid projectId,
        Guid lineId,
        ReceiveLineRequest request,
        CancellationToken cancellationToken
    ) =>
        TransitionPackingAsync(
            projectId,
            lineId,
            request.Actor,
            null,
            PackingTransition.Receive,
            static (project, id, actor, at) => project.Receive(id, actor, at),
            cancellationToken
        );

    private async Task<PackingLineResponse?> TransitionPackingAsync(
        Guid projectId,
        Guid lineId,
        string actor,
        string? scanCode,
        PackingTransition packingTransition,
        Func<Project, Guid, string, DateTimeOffset, PackingLine> transition,
        CancellationToken cancellationToken
    )
    {
        var project = await projects.FindAsync(projectId, cancellationToken).ConfigureAwait(false);
        if (project is null)
            return null;
        var plannedLine = project.PackingLines.FirstOrDefault(line => line.Id == lineId);
        if (plannedLine is null)
            return null;
        if (packingTransition == PackingTransition.BookOut)
            await equipment
                .BookOutAsync(
                    plannedLine.EquipmentId,
                    plannedLine.Quantity,
                    scanCode,
                    cancellationToken
                )
                .ConfigureAwait(false);
        else if (packingTransition == PackingTransition.Receive)
            await equipment
                .ReceiveAsync(plannedLine.EquipmentId, plannedLine.Quantity, cancellationToken)
                .ConfigureAwait(false);
        var line = transition(project, lineId, actor, DateTimeOffset.UtcNow);
        await unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        return ToResponse(line);
    }

    private enum PackingTransition
    {
        Pack,
        BookOut,
        Receive,
    }

    private static PackingLineRole ParsePackingRole(string role) =>
        Enum.TryParse<PackingLineRole>(role, true, out var parsed)
            ? parsed
            : throw new BusinessRuleViolationException(
                "projects.packing-role-invalid",
                $"Unknown packing line role '{role}'."
            );

    private static ProjectResponse ToResponse(Project project, string customerName) =>
        new(
            project.Id,
            project.Name,
            project.CustomerId,
            customerName,
            project.StartDate,
            project.EndDate,
            Currency.Eur.Code,
            [.. project.BillableLines.Select(ToResponse)],
            project.ProjectTypeId,
            project.TeamId,
            [.. project.PackingLines.Select(ToResponse)],
            project.RefundableDepositAmount,
            (Raccord.Contracts.Projects.DepositStatus)project.DepositStatus
        );

    private static BillableLineResponse ToResponse(BillableLine line) =>
        new(
            line.Id,
            line.Kind.ToString().ToLowerInvariant(),
            line.Description,
            line.Quantity,
            line.UnitPrice,
            line.DiscountPercent,
            line.VatPercentage,
            line.Pricing.NetAmount.Amount,
            line.State switch
            {
                BillingState.Unbilled => "unbilled",
                BillingState.InDraft => "in-draft",
                _ => "invoiced",
            },
            line.DraftId,
            line.InvoiceId,
            line.VatClassId,
            line.VatSchemeId
        );

    private static PackingLineResponse ToResponse(PackingLine line) =>
        new(
            line.Id,
            line.EquipmentId,
            line.Quantity,
            line.Role.ToString().ToLowerInvariant(),
            line.Disposition switch
            {
                PackingLineDisposition.CrossDocked => "cross-docked",
                PackingLineDisposition.SaleReturn => "sale-return",
                _ => "normal-return",
            },
            line.IsPacked,
            line.IsBookedOut,
            line.IsReceived,
            line.PackedBy,
            line.BookedOutBy,
            line.ReceivedBy,
            line.ExceptionKind,
            line.ExceptionDescription,
            line.ExceptionReportedBy,
            line.ExceptionReportedAt
        );
}
