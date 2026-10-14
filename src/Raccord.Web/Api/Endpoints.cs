using Microsoft.AspNetCore.Http.HttpResults;
using Raccord.Application.Auditing;
using Raccord.Contracts.Auditing;
using Raccord.Contracts.Customers;
using Raccord.Contracts.Invoicing;
using Raccord.Contracts.Organization;
using Raccord.Contracts.Projects;
using Raccord.Modules.Contacts;
using Raccord.Modules.Invoicing;
using Raccord.Modules.Projects;
using Raccord.Modules.Workspace;

namespace Raccord.Web.Api;

/// <summary>The public HTTP API. Endpoints only translate HTTP to use cases; they hold no business logic.</summary>
internal static class Endpoints
{
    public static IEndpointRouteBuilder MapRaccordApi(
        this IEndpointRouteBuilder app,
        bool requireAuthorization = true
    )
    {
        var api = app.MapGroup("/api");
        if (requireAuthorization)
        {
            api.RequireAuthorization();
        }
        MapOrganization(api);
        MapCustomers(api);
        MapProjects(api);
        MapInvoicing(api);
        api.MapGet(
                "/audit-events",
                async (Guid entityId, AuditLog audit, CancellationToken ct) =>
                    TypedResults.Ok<IReadOnlyList<AuditEventResponse>>(
                        await audit.ListForEntityAsync(entityId, ct)
                    )
            )
            .WithTags("Audit");
        return app;
    }

    private static void MapOrganization(RouteGroupBuilder api)
    {
        var organization = api.MapGroup("/organization").WithTags("Organization");
        organization.MapGet(
            "/issuer",
            async Task<Results<Ok<IssuerProfileResponse>, NotFound>> (
                OrganizationSettings settings,
                CancellationToken ct
            ) =>
                await settings.GetIssuerAsync(ct) is { } issuer
                    ? TypedResults.Ok(issuer)
                    : TypedResults.NotFound()
        );
        organization.MapPut(
            "/issuer",
            async (
                UpdateIssuerProfileRequest request,
                OrganizationSettings settings,
                CancellationToken ct
            ) => TypedResults.Ok(await settings.UpdateIssuerAsync(request, ct))
        );
        api.MapGet(
                "/invoice-series",
                async (OrganizationSettings settings, CancellationToken ct) =>
                    TypedResults.Ok(await settings.ListSeriesAsync(ct))
            )
            .WithTags("Organization");
    }

    private static void MapCustomers(RouteGroupBuilder api)
    {
        var customers = api.MapGroup("/customers").WithTags("Customers");
        customers.MapGet(
            "/",
            async (CustomerDirectory directory, CancellationToken ct) =>
                TypedResults.Ok(await directory.ListAsync(ct))
        );
        customers.MapGet(
            "/{id:guid}",
            async Task<Results<Ok<CustomerResponse>, NotFound>> (
                Guid id,
                CustomerDirectory directory,
                CancellationToken ct
            ) =>
                await directory.GetAsync(id, ct) is { } customer
                    ? TypedResults.Ok(customer)
                    : TypedResults.NotFound()
        );
        customers.MapPost(
            "/",
            async (
                RegisterCustomerRequest request,
                CustomerDirectory directory,
                CancellationToken ct
            ) =>
            {
                var customer = await directory.RegisterAsync(request, ct);
                return TypedResults.Created($"/api/customers/{customer.Id}", customer);
            }
        );
    }

    private static void MapProjects(RouteGroupBuilder api)
    {
        var projects = api.MapGroup("/projects").WithTags("Projects");
        var coordination = api.MapGroup("/project-coordination").WithTags("Project coordination");
        MapProjectCoordination(coordination);
        projects.MapGet(
            "/",
            async (ProjectWorkspace workspace, CancellationToken ct) =>
                TypedResults.Ok(await workspace.ListAsync(ct))
        );
        projects.MapGet(
            "/{id:guid}",
            async Task<Results<Ok<ProjectResponse>, NotFound>> (
                Guid id,
                ProjectWorkspace workspace,
                CancellationToken ct
            ) =>
                await workspace.GetAsync(id, ct) is { } project
                    ? TypedResults.Ok(project)
                    : TypedResults.NotFound()
        );
        projects.MapPost(
            "/",
            async (
                CreateProjectRequest request,
                ProjectWorkspace workspace,
                CancellationToken ct
            ) =>
            {
                var project = await workspace.CreateAsync(request, ct);
                return TypedResults.Created($"/api/projects/{project.Id}", project);
            }
        );
        projects.MapPost(
            "/{id:guid}/billable-lines",
            async Task<Results<Created<BillableLineResponse>, NotFound>> (
                Guid id,
                AddBillableLineRequest request,
                ProjectWorkspace workspace,
                CancellationToken ct
            ) =>
                await workspace.AddBillableLineAsync(id, request, ct) is { } line
                    ? TypedResults.Created($"/api/projects/{id}", line)
                    : TypedResults.NotFound()
        );
    }

    private static void MapProjectCoordination(RouteGroupBuilder coordination)
    {
        MapProjectRequests(coordination);
        MapProjectClassifications(coordination);
        MapProjectBoard(coordination);
        MapProjectSchedules(coordination);
    }

    private static void MapProjectRequests(RouteGroupBuilder coordination)
    {
        coordination.MapGet(
            "/requests",
            async (ProjectCoordination service, CancellationToken ct) =>
                TypedResults.Ok(await service.ListRequestsAsync(ct))
        );
        coordination.MapPost(
            "/requests",
            async (
                ReceiveProjectRequest request,
                ProjectCoordination service,
                CancellationToken ct
            ) =>
            {
                var result = await service.ReceiveAsync(request, ct);
                return TypedResults.Created(
                    $"/api/project-coordination/requests/{result.Id}",
                    result
                );
            }
        );
        coordination.MapPost(
            "/requests/{id:guid}/convert",
            async Task<Results<Created<ProjectResponse>, NotFound>> (
                Guid id,
                ProjectCoordination service,
                CancellationToken ct
            ) =>
            {
                var result = await service.ConvertAsync(id, ct);
                return result is null
                    ? TypedResults.NotFound()
                    : TypedResults.Created($"/api/projects/{result.Id}", result);
            }
        );
        coordination.MapPost(
            "/requests/{id:guid}/decline",
            async Task<Results<Ok<ProjectRequestResponse>, NotFound>> (
                Guid id,
                DeclineProjectRequest request,
                ProjectCoordination service,
                CancellationToken ct
            ) =>
                await service.DeclineAsync(id, request, ct) is { } result
                    ? TypedResults.Ok(result)
                    : TypedResults.NotFound()
        );
    }

    private static void MapProjectClassifications(RouteGroupBuilder coordination)
    {
        coordination.MapGet(
            "/types",
            async (ProjectCoordination service, CancellationToken ct) =>
                TypedResults.Ok(await service.ListTypesAsync(ct))
        );
        coordination.MapPost(
            "/types",
            async (
                CreateProjectClassificationRequest request,
                ProjectCoordination service,
                CancellationToken ct
            ) =>
                TypedResults.Created(
                    "/api/project-coordination/types",
                    await service.CreateTypeAsync(request, ct)
                )
        );
        coordination.MapGet(
            "/teams",
            async (ProjectCoordination service, CancellationToken ct) =>
                TypedResults.Ok(await service.ListTeamsAsync(ct))
        );
        coordination.MapPost(
            "/teams",
            async (
                CreateProjectClassificationRequest request,
                ProjectCoordination service,
                CancellationToken ct
            ) =>
                TypedResults.Created(
                    "/api/project-coordination/teams",
                    await service.CreateTeamAsync(request, ct)
                )
        );
    }

    private static void MapProjectBoard(RouteGroupBuilder coordination)
    {
        coordination.MapGet(
            "/statuses",
            async (ProjectCoordination service, CancellationToken ct) =>
                TypedResults.Ok(await service.ListTaskStatusesAsync(ct))
        );
        coordination.MapPost(
            "/statuses",
            async (
                CreateProjectTaskStatusRequest request,
                ProjectCoordination service,
                CancellationToken ct
            ) =>
                TypedResults.Created(
                    "/api/project-coordination/statuses",
                    await service.CreateTaskStatusAsync(request, ct)
                )
        );
        coordination.MapGet(
            "/projects/{projectId:guid}/board",
            async Task<Results<Ok<ProjectBoardResponse>, NotFound>> (
                Guid projectId,
                ProjectCoordination service,
                CancellationToken ct
            ) =>
                await service.GetBoardAsync(projectId, ct) is { } board
                    ? TypedResults.Ok(board)
                    : TypedResults.NotFound()
        );
        coordination.MapPost(
            "/tasks",
            async (
                CreateProjectTaskRequest request,
                ProjectCoordination service,
                CancellationToken ct
            ) =>
                TypedResults.Created(
                    "/api/project-coordination/tasks",
                    await service.CreateTaskAsync(request, ct)
                )
        );
        coordination.MapPost(
            "/tasks/{id:guid}/progress",
            async Task<Results<Ok<ProjectTaskResponse>, NotFound>> (
                Guid id,
                ProgressProjectTaskRequest request,
                ProjectCoordination service,
                CancellationToken ct
            ) =>
                await service.ProgressTaskAsync(id, request, ct) is { } task
                    ? TypedResults.Ok(task)
                    : TypedResults.NotFound()
        );
    }

    private static void MapProjectSchedules(RouteGroupBuilder coordination)
    {
        coordination.MapGet(
            "/schedules",
            async (
                Guid? projectId,
                Guid? assigneeId,
                ProjectCoordination service,
                CancellationToken ct
            ) => TypedResults.Ok(await service.ListSchedulesAsync(projectId, assigneeId, ct))
        );
        coordination.MapPost(
            "/schedules",
            async (
                CreateProjectScheduleRequest request,
                ProjectCoordination service,
                CancellationToken ct
            ) =>
                TypedResults.Created(
                    "/api/project-coordination/schedules",
                    await service.CreateScheduleAsync(request, ct)
                )
        );
        coordination.MapPost(
            "/schedules/{id:guid}/shift",
            async Task<Results<Ok<ProjectScheduleResponse>, NotFound>> (
                Guid id,
                ShiftProjectScheduleRequest request,
                ProjectCoordination service,
                CancellationToken ct
            ) =>
                await service.ShiftScheduleAsync(id, request, ct) is { } schedule
                    ? TypedResults.Ok(schedule)
                    : TypedResults.NotFound()
        );
    }

    private static void MapInvoicing(RouteGroupBuilder api)
    {
        MapInvoiceDrafts(api);
        MapInvoices(api);
    }

    private static void MapInvoiceDrafts(RouteGroupBuilder api)
    {
        var drafts = api.MapGroup("/invoice-drafts").WithTags("Invoicing");
        drafts.MapGet(
            "/",
            async (InvoicingWorkflow invoicing, CancellationToken ct) =>
                TypedResults.Ok(await invoicing.ListDraftsAsync(ct))
        );
        drafts.MapGet(
            "/{id:guid}",
            async Task<Results<Ok<InvoiceDraftResponse>, NotFound>> (
                Guid id,
                InvoicingWorkflow invoicing,
                CancellationToken ct
            ) =>
                await invoicing.GetDraftAsync(id, ct) is { } draft
                    ? TypedResults.Ok(draft)
                    : TypedResults.NotFound()
        );
        drafts.MapPost(
            "/",
            async (
                CreateInvoiceDraftRequest request,
                InvoicingWorkflow invoicing,
                CancellationToken ct
            ) =>
            {
                var draft = await invoicing.CreateDraftAsync(request, ct);
                return TypedResults.Created($"/api/invoice-drafts/{draft.Id}", draft);
            }
        );
        MapDraftLineRemoval(drafts);
        MapDraftDiscard(drafts);
        MapDraftFinalize(drafts);
    }

    private static void MapDraftLineRemoval(RouteGroupBuilder drafts)
    {
        drafts.MapDelete(
            "/{id:guid}/lines/{lineId:guid}",
            async Task<Results<Ok<InvoiceDraftResponse>, NotFound>> (
                Guid id,
                Guid lineId,
                InvoicingWorkflow invoicing,
                CancellationToken ct
            ) =>
                await invoicing.RemoveDraftLineAsync(id, lineId, ct) is { } draft
                    ? TypedResults.Ok(draft)
                    : TypedResults.NotFound()
        );
    }

    private static void MapDraftDiscard(RouteGroupBuilder drafts)
    {
        drafts.MapDelete(
            "/{id:guid}",
            async Task<Results<NoContent, NotFound>> (
                Guid id,
                InvoicingWorkflow invoicing,
                CancellationToken ct
            ) =>
                await invoicing.DiscardDraftAsync(id, ct)
                    ? TypedResults.NoContent()
                    : TypedResults.NotFound()
        );
    }

    private static void MapDraftFinalize(RouteGroupBuilder drafts)
    {
        drafts
            .MapPost(
                "/{id:guid}/finalize",
                async Task<Results<Created<IssuedInvoiceResponse>, NotFound>> (
                    Guid id,
                    FinalizeInvoiceDraftRequest request,
                    InvoicingWorkflow invoicing,
                    CancellationToken ct
                ) =>
                    await invoicing.FinalizeAsync(id, request, ct) is { } invoice
                        ? TypedResults.Created($"/api/invoices/{invoice.Id}", invoice)
                        : TypedResults.NotFound()
            )
            .ProducesProblem(StatusCodes.Status422UnprocessableEntity)
            .ProducesProblem(StatusCodes.Status409Conflict);
    }

    private static void MapInvoices(RouteGroupBuilder api)
    {
        var invoices = api.MapGroup("/invoices").WithTags("Invoicing");
        invoices.MapGet(
            "/",
            async (InvoicingWorkflow invoicing, CancellationToken ct) =>
                TypedResults.Ok(await invoicing.ListInvoicesAsync(ct))
        );
        invoices.MapGet(
            "/{id:guid}",
            async Task<Results<Ok<IssuedInvoiceResponse>, NotFound>> (
                Guid id,
                InvoicingWorkflow invoicing,
                CancellationToken ct
            ) =>
                await invoicing.GetInvoiceAsync(id, ct) is { } invoice
                    ? TypedResults.Ok(invoice)
                    : TypedResults.NotFound()
        );
        invoices.MapGet(
            "/{id:guid}/document",
            async Task<Results<FileContentHttpResult, NotFound>> (
                Guid id,
                InvoicingWorkflow invoicing,
                CancellationToken ct
            ) =>
                await invoicing.GetDocumentAsync(id, ct) is { } document
                    ? TypedResults.File(document.Content, document.ContentType, document.FileName)
                    : TypedResults.NotFound()
        );
    }
}
