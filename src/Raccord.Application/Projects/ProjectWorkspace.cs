using Raccord.Application.Abstractions;
using Raccord.Contracts.Projects;
using Raccord.Domain.Projects;
using Raccord.Modules.Contacts.Contracts;
using Raccord.SharedKernel.Abstractions;
using Raccord.SharedKernel.Common;
using Raccord.SharedKernel.Pricing;

namespace Raccord.Application.Projects;

/// <summary>Use cases for projects and their billable work.</summary>
public sealed class ProjectWorkspace(
    IProjectRepository projects,
    IProjectTemplateRepository templates,
    IPartyDirectory parties,
    IUnitOfWork unitOfWork,
    IClock clock
)
{
    /// <summary>Creates a reusable project template.</summary>
    public async Task<ProjectTemplateResponse> CreateTemplateAsync(
        CreateProjectTemplateRequest request,
        CancellationToken ct
    )
    {
        var template = ProjectTemplate.Create(request.Name, request.CustomerId);
        templates.Add(template);
        await unitOfWork.SaveChangesAsync(ct).ConfigureAwait(false);
        return new(template.Id, template.Name, template.CustomerId);
    }

    /// <summary>Creates a project using a stored template's customer.</summary>
    public async Task<ProjectResponse?> CreateFromTemplateAsync(
        CreateProjectFromTemplateRequest request,
        CancellationToken ct
    )
    {
        var template = await templates.FindAsync(request.TemplateId, ct).ConfigureAwait(false);
        if (template is null)
            return null;
        return await CreateAsync(
                new(request.Name, template.CustomerId, request.StartDate, request.EndDate),
                ct
            )
            .ConfigureAwait(false);
    }

    /// <summary>Moves a project and its planned equipment by the requested number of days.</summary>
    public async Task<ProjectResponse?> ShiftAsync(
        Guid id,
        ShiftProjectRequest request,
        CancellationToken ct
    )
    {
        var project = await projects.FindAsync(id, ct).ConfigureAwait(false);
        if (project is null)
            return null;
        project.ShiftDates(request.Days);
        await unitOfWork.SaveChangesAsync(ct).ConfigureAwait(false);
        var customer = await parties.FindAsync(project.CustomerId, ct).ConfigureAwait(false);
        return ToResponse(project, customer?.LegalName ?? string.Empty);
    }

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
            customer.DefaultDiscountPercent,
            customer.DefaultPaymentTerms
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
                    project.DefaultDiscountPercent,
                    project.PaymentTerms,
                    ToContractStatus(project.Status),
                    unbilled.Count,
                    amount.Amount,
                    amount.Currency.Code
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

        var pricing = LinePricing.Create(
            request.Quantity,
            request.UnitPrice,
            request.DiscountPercent,
            VatRate.Of(request.VatRate)
        );
        var line = project.AddBillableLine(kind, request.Description ?? string.Empty, pricing);
        await unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        return line.ToResponse();
    }

    /// <summary>Plans an equipment item for a period within a project.</summary>
    public async Task<PlannedEquipmentLineResponse?> PlanEquipmentAsync(
        Guid projectId,
        PlanEquipmentRequest request,
        CancellationToken ct
    )
    {
        var project = await projects.FindAsync(projectId, ct).ConfigureAwait(false);
        if (project is null)
            return null;
        var line = project.PlanEquipment(
            request.EquipmentId,
            request.Quantity,
            request.StartDate,
            request.EndDate
        );
        await unitOfWork.SaveChangesAsync(ct).ConfigureAwait(false);
        return new(line.Id, line.EquipmentId, line.Quantity, line.StartDate, line.EndDate);
    }

    /// <summary>Creates a warehouse packing list from planned equipment.</summary>
    public async Task<CreatePackingListResponse?> CreatePackingListAsync(
        Guid projectId,
        CancellationToken ct
    )
    {
        var project = await projects.FindAsync(projectId, ct).ConfigureAwait(false);
        if (project is null)
            return null;
        var lines = project.CreatePackingList();
        await unitOfWork.SaveChangesAsync(ct).ConfigureAwait(false);
        return new([.. lines.Select(ToPackingResponse)]);
    }

    /// <summary>Returns a project's packing list.</summary>
    public async Task<CreatePackingListResponse?> GetPackingListAsync(
        Guid projectId,
        CancellationToken ct
    )
    {
        var project = await projects.FindAsync(projectId, ct).ConfigureAwait(false);
        return project is null ? null : new([.. project.PackingLines.Select(ToPackingResponse)]);
    }

    /// <summary>Marks a packing line as packed.</summary>
    public async Task<PackingLineResponse?> PackAsync(
        Guid projectId,
        Guid lineId,
        PackLineRequest request,
        CancellationToken ct
    )
    {
        var project = await projects.FindAsync(projectId, ct).ConfigureAwait(false);
        if (project is null)
            return null;
        var line = project.Pack(lineId, request.PackedBy, clock.UtcNow);
        await unitOfWork.SaveChangesAsync(ct).ConfigureAwait(false);
        return ToPackingResponse(line);
    }

    /// <summary>Books a packed line out of the warehouse.</summary>
    public async Task<PackingLineResponse?> BookOutAsync(
        Guid projectId,
        Guid lineId,
        BookOutLineRequest request,
        CancellationToken ct
    )
    {
        var project = await projects.FindAsync(projectId, ct).ConfigureAwait(false);
        if (project is null)
            return null;
        var line = project.BookOut(lineId, request.BookedOutBy, clock.UtcNow);
        await unitOfWork.SaveChangesAsync(ct).ConfigureAwait(false);
        return ToPackingResponse(line);
    }

    /// <summary>Changes a project's planning lifecycle status.</summary>
    public async Task<ProjectResponse?> SetStatusAsync(
        Guid id,
        Contracts.Projects.ProjectStatus status,
        CancellationToken ct
    )
    {
        var project = await projects.FindAsync(id, ct).ConfigureAwait(false);
        if (project is null)
            return null;
        project.SetStatus(Enum.Parse<Raccord.Domain.Projects.ProjectStatus>(status.ToString()));
        await unitOfWork.SaveChangesAsync(ct).ConfigureAwait(false);
        var customer = await parties.FindAsync(project.CustomerId, ct).ConfigureAwait(false);
        return ToResponse(project, customer?.LegalName ?? string.Empty);
    }

    private static ProjectResponse ToResponse(Project project, string customerName) =>
        new(
            project.Id,
            project.Name,
            project.CustomerId,
            customerName,
            project.StartDate,
            project.EndDate,
            project.DefaultDiscountPercent,
            project.PaymentTerms,
            ToContractStatus(project.Status),
            Currency.Eur.Code,
            [.. project.BillableLines.Select(line => line.ToResponse())],
            [
                .. project.PlannedEquipmentLines.Select(line => new PlannedEquipmentLineResponse(
                    line.Id,
                    line.EquipmentId,
                    line.Quantity,
                    line.StartDate,
                    line.EndDate
                )),
            ],
            [.. project.PackingLines.Select(ToPackingResponse)]
        );

    private static PackingLineResponse ToPackingResponse(PackingLine line) =>
        new(
            line.Id,
            line.EquipmentId,
            line.Quantity,
            line.IsPacked,
            line.PackedBy,
            line.PackedAt,
            line.IsBookedOut,
            line.BookedOutBy,
            line.BookedOutAt
        );

    private static Raccord.Contracts.Projects.ProjectStatus ToContractStatus(
        Raccord.Domain.Projects.ProjectStatus status
    ) => Enum.Parse<Raccord.Contracts.Projects.ProjectStatus>(status.ToString());
}
