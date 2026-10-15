using Microsoft.AspNetCore.Http.HttpResults;
using Raccord.Application.Auditing;
using Raccord.Contracts.Auditing;
using Raccord.Contracts.Customers;
using Raccord.Contracts.Documents;
using Raccord.Contracts.Equipment;
using Raccord.Contracts.Invoicing;
using Raccord.Contracts.Organization;
using Raccord.Contracts.Projects;
using Raccord.Infrastructure.Persistence;
using Raccord.Modules.Contacts;
using Raccord.Modules.Invoicing;
using Raccord.Modules.Projects;
using Raccord.Modules.Workspace;
using Raccord.SharedKernel.Abstractions;

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
        MapDocuments(api);
        MapGeneratedDocuments(api);
        MapCustomers(api);
        MapEquipment(api);
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

#pragma warning disable MA0051
    private static void MapDocuments(RouteGroupBuilder api)
    {
        var documents = api.MapGroup("/documents").WithTags("Documents");
        documents.MapGet(
            "/templates",
            async (string? language, DocumentTemplateCatalog catalog, CancellationToken ct) =>
                TypedResults.Ok(await catalog.ListAsync(language, ct))
        );
        documents.MapGet(
            "/templates/{id:guid}",
            async Task<Results<Ok<DocumentTemplateResponse>, NotFound>> (
                Guid id,
                DocumentTemplateCatalog catalog,
                CancellationToken ct
            ) =>
                await catalog.FindAsync(id, ct) is { } template
                    ? TypedResults.Ok(template)
                    : TypedResults.NotFound()
        );
        documents.MapPost(
            "/templates",
            async (
                CreateDocumentTemplateRequest request,
                DocumentTemplateCatalog catalog,
                IClock clock,
                CancellationToken ct
            ) =>
                TypedResults.Created(
                    "/api/documents/templates",
                    await catalog.CreateAsync(request, clock.UtcNow, ct)
                )
        );
        documents.MapPut(
            "/templates/{id:guid}",
            async Task<Results<Ok<DocumentTemplateResponse>, NotFound>> (
                Guid id,
                UpdateDocumentTemplateRequest request,
                DocumentTemplateCatalog catalog,
                IClock clock,
                CancellationToken ct
            ) =>
                await catalog.UpdateAsync(id, request, clock.UtcNow, ct) is { } template
                    ? TypedResults.Ok(template)
                    : TypedResults.NotFound()
        );
        documents.MapGet(
            "/templates/{id:guid}/preview",
            async Task<Results<Ok<DocumentTemplatePreviewResponse>, NotFound>> (
                Guid id,
                DocumentTemplateCatalog catalog,
                CancellationToken ct
            ) =>
                await catalog.PreviewAsync(id, ct) is { } preview
                    ? TypedResults.Ok(preview)
                    : TypedResults.NotFound()
        );
        documents.MapGet(
            "/communications",
            async (string? search, CommunicationCatalog catalog, CancellationToken ct) =>
                TypedResults.Ok(await catalog.ListAsync(search, ct))
        );
        documents.MapPost(
            "/communications",
            async Task<Results<Created<CommunicationSnapshot>, BadRequest<string>>> (
                CreateCommunicationRequest request,
                CommunicationCatalog catalog,
                CancellationToken ct
            ) =>
                await catalog.CreateAsync(request, ct) is { } communication
                    ? TypedResults.Created(
                        $"/api/documents/communications/{communication.Id}",
                        communication
                    )
                    : TypedResults.BadRequest<string>("Recipient and subject are required.")
        );
        documents.MapPut(
            "/communications/{id:guid}/delivery",
            async Task<Results<Ok<CommunicationSnapshot>, NotFound>> (
                Guid id,
                RecordDeliveryOutcomeRequest request,
                CommunicationCatalog catalog,
                CancellationToken ct
            ) =>
                await catalog.RecordOutcomeAsync(id, request, ct) is { } communication
                    ? TypedResults.Ok(communication)
                    : TypedResults.NotFound()
        );
    }
#pragma warning restore MA0051

    private static void MapGeneratedDocuments(RouteGroupBuilder api)
    {
        api.MapGet(
                "/projects/{id:guid}/document",
                async Task<Results<Ok<GeneratedProjectDocumentResponse>, NotFound>> (
                    Guid id,
                    DocumentGenerationCatalog catalog,
                    CancellationToken ct
                ) =>
                    await catalog.GenerateProjectAsync(id, ct) is { } document
                        ? TypedResults.Ok(document)
                        : TypedResults.NotFound()
            )
            .WithTags("Documents");
        api.MapGet(
                "/equipment/items/{id:guid}/label",
                async Task<Results<Ok<EquipmentLabelResponse>, NotFound>> (
                    Guid id,
                    DocumentGenerationCatalog catalog,
                    CancellationToken ct
                ) =>
                    await catalog.GenerateLabelAsync(id, ct) is { } label
                        ? TypedResults.Ok(label)
                        : TypedResults.NotFound()
            )
            .WithTags("Documents");
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

    private static void MapEquipment(RouteGroupBuilder api)
    {
        var equipment = api.MapGroup("/equipment").WithTags("Equipment");
        equipment.MapGet(
            "/items",
            async (EquipmentCatalog catalog, CancellationToken ct) =>
                TypedResults.Ok(await catalog.ListAsync(ct))
        );
        equipment.MapGet(
            "/items/{id:guid}",
            async Task<Results<Ok<EquipmentItemResponse>, NotFound>> (
                Guid id,
                EquipmentCatalog catalog,
                CancellationToken ct
            ) =>
                (await catalog.FindAsync(id, ct)) is { } item
                    ? TypedResults.Ok(item)
                    : TypedResults.NotFound()
        );
        equipment.MapPost(
            "/items",
            async (
                CreateEquipmentItemRequest request,
                EquipmentCatalog catalog,
                CancellationToken ct
            ) =>
            {
                var item = await catalog.CreateAsync(request, ct);
                return TypedResults.Created($"/api/equipment/items/{item.Id}", item);
            }
        );
        equipment.MapPut(
            "/items/{id:guid}/scan-code",
            async Task<Results<Ok<EquipmentItemResponse>, NotFound>> (
                Guid id,
                AssignEquipmentScanCodeRequest request,
                EquipmentCatalog catalog,
                CancellationToken ct
            ) =>
                await catalog.AssignScanCodeAsync(id, request, ct) is { } item
                    ? TypedResults.Ok(item)
                    : TypedResults.NotFound()
        );
        MapEquipmentStockActions(equipment);
        MapEquipmentLifecycle(equipment);
    }

    private static void MapEquipmentStockActions(RouteGroupBuilder equipment)
    {
        equipment.MapPost(
            "/items/{id:guid}/movements",
            async Task<Results<Created<StockMovementResponse>, NotFound>> (
                Guid id,
                ScheduleStockMovementRequest request,
                EquipmentCatalog catalog,
                IClock clock,
                CancellationToken ct
            ) =>
                await catalog.ScheduleMovementAsync(id, request, clock.UtcNow, ct) is { } movement
                    ? TypedResults.Created(
                        $"/api/equipment/items/{id}/movements/{movement.Id}",
                        movement
                    )
                    : TypedResults.NotFound()
        );
        equipment.MapGet(
            "/items/{id:guid}/movements",
            async (Guid id, EquipmentCatalog catalog, CancellationToken ct) =>
                TypedResults.Ok(await catalog.ListMovementsAsync(id, ct))
        );
        equipment.MapPost(
            "/items/{id:guid}/counts",
            async Task<Results<Created<StockCountResponse>, NotFound>> (
                Guid id,
                ReconcileStockCountRequest request,
                EquipmentCatalog catalog,
                IClock clock,
                CancellationToken ct
            ) =>
                await catalog.ReconcileCountAsync(id, request, clock.UtcNow, ct) is { } count
                    ? TypedResults.Created($"/api/equipment/items/{id}/counts/{count.Id}", count)
                    : TypedResults.NotFound()
        );
        equipment.MapGet(
            "/items/{id:guid}/counts",
            async (Guid id, EquipmentCatalog catalog, CancellationToken ct) =>
                TypedResults.Ok(await catalog.ListCountsAsync(id, ct))
        );
    }

    private static void MapEquipmentLifecycle(RouteGroupBuilder equipment)
    {
        equipment.MapPut(
            "/items/{id:guid}/retire",
            async Task<Results<Ok<EquipmentItemResponse>, NotFound>> (
                Guid id,
                RetireEquipmentRequest request,
                EquipmentCatalog catalog,
                IClock clock,
                CancellationToken ct
            ) =>
                await catalog.RetireAsync(id, request, clock.UtcNow, ct) is { } item
                    ? TypedResults.Ok(item)
                    : TypedResults.NotFound()
        );
        equipment.MapPut(
            "/items/{id:guid}/sell",
            async Task<Results<Ok<EquipmentItemResponse>, NotFound>> (
                Guid id,
                SellEquipmentRequest request,
                EquipmentCatalog catalog,
                IClock clock,
                CancellationToken ct
            ) =>
                await catalog.SellAsync(id, request, clock.UtcNow, ct) is { } item
                    ? TypedResults.Ok(item)
                    : TypedResults.NotFound()
        );
        equipment.MapGet(
            "/items/{id:guid}/history",
            async (Guid id, EquipmentCatalog catalog, CancellationToken ct) =>
                TypedResults.Ok(await catalog.ListHistoryAsync(id, ct))
        );
        equipment.MapGet(
            "/items/{id:guid}/insights",
            async Task<Results<Ok<EquipmentInsightsResponse>, NotFound>> (
                Guid id,
                EquipmentCatalog catalog,
                CancellationToken ct
            ) =>
                await catalog.GetInsightsAsync(id, ct) is { } insights
                    ? TypedResults.Ok(insights)
                    : TypedResults.NotFound()
        );
    }

#pragma warning disable MA0051
    private static void MapProjects(RouteGroupBuilder api)
    {
        var projects = api.MapGroup("/projects").WithTags("Projects");
        var coordination = api.MapGroup("/project-coordination").WithTags("Project coordination");
        MapProjectCoordination(coordination);
        MapProjectContext(projects);
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
        projects.MapPut(
            "/{id:guid}/deposit",
            async Task<Results<Ok<ProjectResponse>, NotFound>> (
                Guid id,
                SetRefundableDepositRequest request,
                ProjectWorkspace workspace,
                CancellationToken ct
            ) =>
                await workspace.SetDepositAsync(id, request, ct) is { } project
                    ? TypedResults.Ok(project)
                    : TypedResults.NotFound()
        );
        projects.MapPut(
            "/{id:guid}/deposit/status",
            async Task<Results<Ok<ProjectResponse>, NotFound>> (
                Guid id,
                SetDepositStatusRequest request,
                ProjectWorkspace workspace,
                CancellationToken ct
            ) =>
                await workspace.SetDepositStatusAsync(id, request, ct) is { } project
                    ? TypedResults.Ok(project)
                    : TypedResults.NotFound()
        );
        MapPackingLines(projects);
    }

    private static void MapProjectContext(RouteGroupBuilder projects)
    {
        projects.MapGet(
            "/{id:guid}/context",
            async Task<Results<Ok<ProjectContextResponse>, NotFound>> (
                Guid id,
                ProjectContext context,
                CancellationToken ct
            ) =>
                await context.GetAsync(id, ct) is { } response
                    ? TypedResults.Ok(response)
                    : TypedResults.NotFound()
        );
        projects.MapPost(
            "/{id:guid}/context/notes",
            async Task<Results<Created<ProjectContextEntryResponse>, NotFound>> (
                Guid id,
                AddProjectNoteRequest request,
                ProjectContext context,
                CancellationToken ct
            ) =>
                await context.AddNoteAsync(id, request, ct) is { } entry
                    ? TypedResults.Created($"/api/projects/{id}/context/{entry.Id}", entry)
                    : TypedResults.NotFound()
        );
        projects.MapPost(
            "/{id:guid}/context/files",
            async Task<Results<Created<ProjectContextEntryResponse>, NotFound>> (
                Guid id,
                AddProjectFileRequest request,
                ProjectContext context,
                CancellationToken ct
            ) =>
                await context.AddFileAsync(id, request, ct) is { } entry
                    ? TypedResults.Created($"/api/projects/{id}/context/{entry.Id}", entry)
                    : TypedResults.NotFound()
        );
        projects.MapPost(
            "/{id:guid}/context/folders",
            async Task<Results<Created<ProjectContextEntryResponse>, NotFound>> (
                Guid id,
                AddProjectFolderRequest request,
                ProjectContext context,
                CancellationToken ct
            ) =>
                await context.AddFolderAsync(id, request, ct) is { } entry
                    ? TypedResults.Created($"/api/projects/{id}/context/{entry.Id}", entry)
                    : TypedResults.NotFound()
        );
        projects.MapPost(
            "/{id:guid}/context/fields",
            async Task<Results<Created<ProjectContextEntryResponse>, NotFound>> (
                Guid id,
                DefineProjectFieldRequest request,
                ProjectContext context,
                CancellationToken ct
            ) =>
                await context.DefineFieldAsync(id, request, ct) is { } entry
                    ? TypedResults.Created($"/api/projects/{id}/context/{entry.Id}", entry)
                    : TypedResults.NotFound()
        );
        projects.MapPost(
            "/{id:guid}/context/tags",
            async Task<Results<Created<ProjectContextEntryResponse>, NotFound>> (
                Guid id,
                TagProjectRecordRequest request,
                ProjectContext context,
                CancellationToken ct
            ) =>
                await context.AddTagAsync(id, request, ct) is { } entry
                    ? TypedResults.Created($"/api/projects/{id}/context/{entry.Id}", entry)
                    : TypedResults.NotFound()
        );
        projects.MapPost(
            "/{id:guid}/context/views",
            async Task<Results<Created<ProjectContextEntryResponse>, NotFound>> (
                Guid id,
                SaveProjectViewRequest request,
                ProjectContext context,
                CancellationToken ct
            ) =>
                await context.SaveViewAsync(id, request, ct) is { } entry
                    ? TypedResults.Created($"/api/projects/{id}/context/{entry.Id}", entry)
                    : TypedResults.NotFound()
        );
    }

    private static void MapPackingLines(RouteGroupBuilder projects)
    {
        projects.MapPost(
            "/{id:guid}/packing-lines",
            async Task<Results<Created<PackingLineResponse>, NotFound>> (
                Guid id,
                PlanEquipmentRequest request,
                ProjectWorkspace workspace,
                CancellationToken ct
            ) =>
                await workspace.AddPackingLineAsync(id, request, ct) is { } line
                    ? TypedResults.Created($"/api/projects/{id}/packing-lines/{line.Id}", line)
                    : TypedResults.NotFound()
        );
        MapPackingAdjustments(projects);
        MapPackingExceptions(projects);
        projects.MapPost(
            "/{id:guid}/packing-lines/{lineId:guid}/pack",
            async Task<Results<Ok<PackingLineResponse>, NotFound>> (
                Guid id,
                Guid lineId,
                PackLineRequest request,
                ProjectWorkspace workspace,
                CancellationToken ct
            ) =>
                await workspace.PackAsync(id, lineId, request, ct) is { } line
                    ? TypedResults.Ok(line)
                    : TypedResults.NotFound()
        );
        projects.MapPost(
            "/{id:guid}/packing-lines/{lineId:guid}/book-out",
            async Task<Results<Ok<PackingLineResponse>, NotFound>> (
                Guid id,
                Guid lineId,
                BookOutLineRequest request,
                ProjectWorkspace workspace,
                CancellationToken ct
            ) =>
                await workspace.BookOutAsync(id, lineId, request, ct) is { } line
                    ? TypedResults.Ok(line)
                    : TypedResults.NotFound()
        );
        projects.MapPost(
            "/{id:guid}/packing-lines/{lineId:guid}/receive",
            async Task<Results<Ok<PackingLineResponse>, NotFound>> (
                Guid id,
                Guid lineId,
                ReceiveLineRequest request,
                ProjectWorkspace workspace,
                CancellationToken ct
            ) =>
                await workspace.ReceiveAsync(id, lineId, request, ct) is { } line
                    ? TypedResults.Ok(line)
                    : TypedResults.NotFound()
        );
    }
#pragma warning restore MA0051

    private static void MapPackingAdjustments(RouteGroupBuilder projects)
    {
        projects.MapPatch(
            "/{id:guid}/packing-lines/{lineId:guid}",
            async Task<Results<Ok<PackingLineResponse>, NotFound>> (
                Guid id,
                Guid lineId,
                AdjustPackingLineRequest request,
                ProjectWorkspace workspace,
                CancellationToken ct
            ) =>
                await workspace.AdjustPackingLineAsync(id, lineId, request, ct) is { } line
                    ? TypedResults.Ok(line)
                    : TypedResults.NotFound()
        );
    }

    private static void MapPackingExceptions(RouteGroupBuilder projects)
    {
        projects.MapPost(
            "/{id:guid}/packing-lines/{lineId:guid}/exceptions",
            async Task<Results<Ok<PackingLineResponse>, NotFound>> (
                Guid id,
                Guid lineId,
                RecordPackingExceptionRequest request,
                ProjectWorkspace workspace,
                CancellationToken ct
            ) =>
                await workspace.RecordExceptionAsync(id, lineId, request, ct) is { } line
                    ? TypedResults.Ok(line)
                    : TypedResults.NotFound()
        );
        projects.MapPost(
            "/{id:guid}/packing-lines/{lineId:guid}/disposition",
            async Task<Results<Ok<PackingLineResponse>, NotFound>> (
                Guid id,
                Guid lineId,
                ReviewPackingDispositionRequest request,
                ProjectWorkspace workspace,
                CancellationToken ct
            ) =>
                await workspace.ReviewDispositionAsync(id, lineId, request, ct) is { } line
                    ? TypedResults.Ok(line)
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
        MapFinance(api);
    }

    private static void MapFinance(RouteGroupBuilder api)
    {
        var finance = api.MapGroup("/finance").WithTags("Finance");
        finance.MapGet(
            "/report",
            async (
                DateOnly? from,
                DateOnly? to,
                InvoicingWorkflow invoicing,
                CancellationToken ct
            ) => TypedResults.Ok(await invoicing.GetFinanceReportAsync(from, to, ct))
        );
        finance.MapGet(
            "/ledger",
            async (
                DateOnly? from,
                DateOnly? to,
                InvoicingWorkflow invoicing,
                CancellationToken ct
            ) => TypedResults.Ok(await invoicing.ListFinanceLedgerAsync(from, to, ct))
        );
        finance.MapGet(
            "/accounting-export",
            async Task<FileContentHttpResult> (
                DateOnly? from,
                DateOnly? to,
                InvoicingWorkflow invoicing,
                CancellationToken ct
            ) =>
                TypedResults.File(
                    await invoicing.ExportFinanceLedgerAsync(from, to, ct),
                    "text/csv",
                    $"raccord-ledger-{from ?? DateOnly.MinValue:yyyyMMdd}-{to ?? DateOnly.MaxValue:yyyyMMdd}.csv"
                )
        );
        finance.MapGet(
            "/invoices-export",
            async Task<FileContentHttpResult> (
                DateOnly? from,
                DateOnly? to,
                InvoicingWorkflow invoicing,
                CancellationToken ct
            ) =>
                TypedResults.File(
                    await invoicing.ExportInvoiceRegisterAsync(from, to, ct),
                    "text/csv",
                    $"raccord-invoices-{from ?? DateOnly.MinValue:yyyyMMdd}-{to ?? DateOnly.MaxValue:yyyyMMdd}.csv"
                )
        );
    }

    private static void MapInvoiceDrafts(RouteGroupBuilder api)
    {
        api.MapGet(
                "/invoicing/queue",
                async (
                    DateOnly? from,
                    DateOnly? to,
                    InvoicingWorkflow invoicing,
                    CancellationToken ct
                ) => TypedResults.Ok(await invoicing.ListQueueAsync(from, to, ct))
            )
            .WithTags("Invoicing");
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
        drafts.MapPost(
            "/standalone",
            async (
                CreateStandaloneInvoiceRequest request,
                InvoicingWorkflow invoicing,
                CancellationToken ct
            ) =>
            {
                var draft = await invoicing.CreateStandaloneDraftAsync(request, ct);
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

#pragma warning disable MA0051
    private static void MapInvoices(RouteGroupBuilder api)
    {
        var invoices = api.MapGroup("/invoices").WithTags("Invoicing");
        invoices.MapGet(
            "/",
            async (InvoicingWorkflow invoicing, CancellationToken ct) =>
                TypedResults.Ok(await invoicing.ListInvoicesAsync(ct))
        );
        MapInvoiceReminders(invoices);
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
            "/{id:guid}/payments",
            async Task<Results<Ok<InvoicePaymentSummaryResponse>, NotFound>> (
                Guid id,
                InvoicingWorkflow invoicing,
                CancellationToken ct
            ) =>
                await invoicing.ListPaymentsAsync(id, ct) is { } payments
                    ? TypedResults.Ok(payments)
                    : TypedResults.NotFound()
        );
        invoices.MapPost(
            "/{id:guid}/payments",
            async Task<Results<Created<InvoicePaymentSummaryResponse>, NotFound>> (
                Guid id,
                RecordInvoicePaymentRequest request,
                InvoicingWorkflow invoicing,
                CancellationToken ct
            ) =>
                await invoicing.RecordPaymentAsync(id, request, ct) is { } payments
                    ? TypedResults.Created($"/api/invoices/{id}/payments", payments)
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
        invoices.MapPost(
            "/{id:guid}/duplicate",
            async Task<Results<Created<InvoiceDraftResponse>, NotFound>> (
                Guid id,
                InvoicingWorkflow invoicing,
                CancellationToken ct
            ) =>
                await invoicing.DuplicateInvoiceAsync(id, ct) is { } draft
                    ? TypedResults.Created($"/api/invoice-drafts/{draft.Id}", draft)
                    : TypedResults.NotFound()
        );
    }
#pragma warning restore MA0051

    private static void MapInvoiceReminders(RouteGroupBuilder invoices)
    {
        invoices.MapGet(
            "/overdue",
            async (InvoicingWorkflow invoicing, CancellationToken ct) =>
                TypedResults.Ok(await invoicing.ListOverdueInvoicesAsync(ct))
        );
        invoices.MapGet(
            "/{id:guid}/reminders",
            async Task<Results<Ok<IReadOnlyList<InvoiceReminderResponse>>, NotFound>> (
                Guid id,
                InvoicingWorkflow invoicing,
                CancellationToken ct
            ) =>
                await invoicing.ListRemindersAsync(id, ct) is { } reminders
                    ? TypedResults.Ok(reminders)
                    : TypedResults.NotFound()
        );
        invoices
            .MapPost(
                "/{id:guid}/reminders",
                async Task<Results<Created<InvoiceReminderResponse>, NotFound>> (
                    Guid id,
                    CreateInvoiceReminderRequest request,
                    InvoicingWorkflow invoicing,
                    CancellationToken ct
                ) =>
                    await invoicing.CreateReminderAsync(id, request, ct) is { } reminder
                        ? TypedResults.Created(
                            $"/api/invoices/{id}/reminders/{reminder.Id}",
                            reminder
                        )
                        : TypedResults.NotFound()
            )
            .ProducesProblem(StatusCodes.Status422UnprocessableEntity);
    }
}
