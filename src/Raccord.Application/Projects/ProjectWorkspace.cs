using Raccord.Application.Abstractions;
using Raccord.Application.Customers;
using Raccord.Contracts.Projects;
using Raccord.Domain.Common;
using Raccord.Domain.Pricing;
using Raccord.Domain.Projects;

namespace Raccord.Application.Projects;

/// <summary>Use cases for projects and their billable work.</summary>
public sealed class ProjectWorkspace(
    IProjectRepository projects,
    ICustomerRepository customers,
    IUnitOfWork unitOfWork
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
            await customers.FindAsync(request.CustomerId, cancellationToken).ConfigureAwait(false)
            ?? throw new BusinessRuleViolationException(
                "customers.not-found",
                $"Customer '{request.CustomerId}' does not exist."
            );
        var project = Project.Create(
            request.Name ?? string.Empty,
            customer.Id,
            request.StartDate,
            request.EndDate
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
        var names = (
            await customers.ListAsync(cancellationToken).ConfigureAwait(false)
        ).ToDictionary(customer => customer.Id, customer => customer.LegalName);
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

        var customer = await customers
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

    private static ProjectResponse ToResponse(Project project, string customerName) =>
        new(
            project.Id,
            project.Name,
            project.CustomerId,
            customerName,
            project.StartDate,
            project.EndDate,
            Currency.Eur.Code,
            [.. project.BillableLines.Select(line => line.ToResponse())]
        );
}
