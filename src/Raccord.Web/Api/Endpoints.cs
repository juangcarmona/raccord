using System.Globalization;
using Microsoft.AspNetCore.Http.HttpResults;
using Raccord.Application.Auditing;
using Raccord.Application.Commercial;
using Raccord.Application.Invoicing;
using Raccord.Application.Organization;
using Raccord.Application.Projects;
using Raccord.Contracts.Auditing;
using Raccord.Contracts.Commercial;
using Raccord.Contracts.Contacts;
using Raccord.Contracts.Customers;
using Raccord.Contracts.Documents;
using Raccord.Contracts.Invoicing;
using Raccord.Contracts.Organization;
using Raccord.Contracts.Projects;
using Raccord.Infrastructure.Documents;
using Raccord.Infrastructure.Persistence;
using Raccord.Modules.Contacts;
using Raccord.Modules.Equipment.Contracts;

namespace Raccord.Web.Api;

/// <summary>The public HTTP API. Endpoints only translate HTTP to use cases; they hold no business logic.</summary>
internal static class Endpoints
{
    public static IEndpointRouteBuilder MapRaccordApi(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("/api");
        MapOrganization(api);
        MapCustomers(api);
        MapContacts(api);
        MapEquipment(api);
        MapDocuments(api);
        MapProjects(api);
        MapInvoicing(api);
        MapCommercial(api);
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
        organization.MapGet(
            "/financial-settings",
            async (OrganizationSettings settings, CancellationToken ct) =>
                TypedResults.Ok(await settings.GetFinancialSettingsAsync(ct))
        );
        organization.MapPut(
            "/financial-settings",
            async (
                UpdateFinancialSettingsRequest request,
                OrganizationSettings settings,
                CancellationToken ct
            ) => TypedResults.Ok(await settings.UpdateFinancialSettingsAsync(request, ct))
        );
        api.MapGet(
                "/invoice-series",
                async (OrganizationSettings settings, CancellationToken ct) =>
                    TypedResults.Ok(await settings.ListSeriesAsync(ct))
            )
            .WithTags("Organization");
    }

#pragma warning disable MA0051
    private static void MapEquipment(RouteGroupBuilder api)
    {
        var equipment = api.MapGroup("/equipment").WithTags("Equipment");
        equipment.MapGet(
            "/items",
            async (int? page, int? pageSize, EquipmentCatalog catalog, CancellationToken ct) =>
                TypedResults.Ok(await catalog.ListAsync(page ?? 1, pageSize ?? 20, ct))
        );
        equipment.MapGet(
            "/items/{id:guid}/availability",
            async Task<Results<Ok<EquipmentAvailabilityResponse>, NotFound>> (
                Guid id,
                DateOnly startDate,
                DateOnly endDate,
                EquipmentCatalog catalog,
                CancellationToken ct
            ) =>
                await catalog.GetAvailabilityAsync(id, startDate, endDate, ct) is { } availability
                    ? TypedResults.Ok(availability)
                    : TypedResults.NotFound()
        );
        equipment.MapGet(
            "/shortages",
            async (EquipmentCatalog catalog, CancellationToken ct) =>
                TypedResults.Ok(await catalog.ListShortagesAsync(ct))
        );
        equipment.MapPost(
            "/shortages/{lineId:guid}/resolve",
            async Task<Results<Ok, NotFound>> (
                Guid lineId,
                Guid alternativeId,
                EquipmentCatalog catalog,
                CancellationToken ct
            ) =>
                await catalog.ResolveShortageAsync(lineId, alternativeId, ct)
                    ? TypedResults.Ok()
                    : TypedResults.NotFound()
        );
        equipment.MapPost(
            "/items",
            async (
                CreateEquipmentItemRequest request,
                EquipmentCatalog catalog,
                CancellationToken ct
            ) =>
                TypedResults.Created(
                    $"/api/equipment/items",
                    await catalog.CreateAsync(request, ct)
                )
        );
        equipment.MapPost(
            "/items/import",
            async (IFormFile file, EquipmentCatalog catalog, CancellationToken ct) =>
            {
                await using var stream = file.OpenReadStream();
                return TypedResults.Ok(await catalog.ImportCsvAsync(stream, ct));
            }
        );
        equipment.MapPut(
            "/items/{id:guid}/details",
            async Task<Results<Ok<EquipmentItemSnapshot>, NotFound>> (
                Guid id,
                UpdateEquipmentItemDetailsRequest request,
                EquipmentCatalog catalog,
                CancellationToken ct
            ) =>
                await catalog.UpdateDetailsAsync(id, request, ct) is { } item
                    ? TypedResults.Ok(item)
                    : TypedResults.NotFound()
        );
        equipment.MapPost("/items/{id:guid}/image", SaveEquipmentImageAsync);
        equipment.MapPut(
            "/items/{id:guid}/kit",
            async Task<Results<Ok<EquipmentItemSnapshot>, NotFound>> (
                Guid id,
                ConfigureEquipmentKitRequest request,
                EquipmentCatalog catalog,
                CancellationToken ct
            ) =>
                await catalog.ConfigureKitAsync(id, request, ct) is { } item
                    ? TypedResults.Ok(item)
                    : TypedResults.NotFound()
        );
        equipment.MapGet(
            "/items/{id:guid}/stock-movements",
            async (Guid id, EquipmentCatalog catalog, CancellationToken ct) =>
                TypedResults.Ok(await catalog.ListStockMovementsAsync(id, ct))
        );
        equipment.MapGet(
            "/items/{id:guid}/label",
            async Task<Results<FileContentHttpResult, NotFound>> (
                Guid id,
                EquipmentCatalog catalog,
                CancellationToken ct
            ) =>
                await catalog.GetAsync(id, ct) is { } item
                    ? TypedResults.File(
                        EquipmentLabelRenderer.Render(item),
                        "application/pdf",
                        $"{item.Code}-label.pdf"
                    )
                    : TypedResults.NotFound()
        );
        equipment.MapPost(
            "/items/{id:guid}/stock-movements",
            async Task<Results<Ok<StockMovementSnapshot>, NotFound>> (
                Guid id,
                ScheduleStockMovementRequest request,
                EquipmentCatalog catalog,
                CancellationToken ct
            ) =>
                await catalog.ScheduleStockMovementAsync(id, request, ct) is { } movement
                    ? TypedResults.Ok(movement)
                    : TypedResults.NotFound()
        );
        equipment.MapPost(
            "/items/{id:guid}/stock-correction",
            async Task<Results<Ok<StockMovementSnapshot>, Conflict, NotFound>> (
                Guid id,
                CorrectStockRequest request,
                EquipmentCatalog catalog,
                CancellationToken ct
            ) =>
                await catalog.CorrectStockAsync(id, request, ct) is { } movement
                    ? TypedResults.Ok(movement)
                    : TypedResults.Conflict()
        );
        equipment.MapPost(
            "/stock-counts",
            async Task<Results<Ok<IReadOnlyList<StockCountResult>>, BadRequest>> (
                CompleteStockCountRequest request,
                EquipmentCatalog catalog,
                CancellationToken ct
            ) =>
                await catalog.CompleteStockCountAsync(request, ct) is { } result
                    ? TypedResults.Ok(result)
                    : TypedResults.BadRequest()
        );
        equipment.MapPost(
            "/items/{id:guid}/sell",
            async Task<Results<Ok<EquipmentSaleResponse>, Conflict>> (
                Guid id,
                SellEquipmentRequest request,
                EquipmentCatalog catalog,
                CancellationToken ct
            ) =>
                await catalog.SellAsync(id, request, ct) is { } sale
                    ? TypedResults.Ok(sale)
                    : TypedResults.Conflict()
        );
        equipment.MapPost(
            "/items/{id:guid}/archive",
            async Task<Results<Ok<EquipmentItemSnapshot>, Conflict>> (
                Guid id,
                int expectedStockQuantity,
                EquipmentCatalog catalog,
                CancellationToken ct
            ) =>
                await catalog.ArchiveAsync(id, expectedStockQuantity, ct) is { } item
                    ? TypedResults.Ok(item)
                    : TypedResults.Conflict()
        );
        equipment.MapGet(
            "/project-history",
            async (Guid[] itemIds, EquipmentCatalog catalog, CancellationToken ct) =>
                TypedResults.Ok(await catalog.ProjectHistoryAsync(itemIds, ct))
        );
    }

    private static void MapDocuments(RouteGroupBuilder api)
    {
        var documents = api.MapGroup("/documents").WithTags("Documents");
        documents.MapGet(
            "/letterheads",
            async (LetterheadCatalog catalog, CancellationToken ct) =>
                TypedResults.Ok(await catalog.ListAsync(ct))
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
            async Task<Results<Ok<CommunicationSnapshot>, NotFound, BadRequest<string>>> (
                Guid id,
                RecordDeliveryOutcomeRequest request,
                CommunicationCatalog catalog,
                CancellationToken ct
            ) =>
                await catalog.RecordOutcomeAsync(id, request, ct) is { } communication
                    ? TypedResults.Ok(communication)
                    : TypedResults.NotFound()
        );
        documents.MapPost(
            "/letterheads",
            async Task<Results<Ok<LetterheadSnapshot>, Conflict, BadRequest<string>>> (
                HttpRequest http,
                LetterheadCatalog catalog,
                CancellationToken ct
            ) =>
            {
                var form = await http.ReadFormAsync(ct);
                var file = form.Files.GetFile("file");
                if (
                    file is null
                    || !string.Equals(
                        file.ContentType,
                        "application/pdf",
                        StringComparison.OrdinalIgnoreCase
                    )
                )
                    return TypedResults.BadRequest<string>("A PDF letterhead file is required.");
                var request = new CreateLetterheadRequest(
                    form["name"].ToString(),
                    form["documentTypes"].ToString(),
                    decimal.TryParse(
                        form["pageWidthMillimetres"],
                        CultureInfo.InvariantCulture,
                        out var width
                    )
                        ? width
                        : 0,
                    decimal.TryParse(
                        form["pageHeightMillimetres"],
                        CultureInfo.InvariantCulture,
                        out var height
                    )
                        ? height
                        : 0,
                    decimal.TryParse(
                        form["marginTopMillimetres"],
                        CultureInfo.InvariantCulture,
                        out var top
                    )
                        ? top
                        : 0,
                    decimal.TryParse(
                        form["marginRightMillimetres"],
                        CultureInfo.InvariantCulture,
                        out var right
                    )
                        ? right
                        : 0,
                    decimal.TryParse(
                        form["marginBottomMillimetres"],
                        CultureInfo.InvariantCulture,
                        out var bottom
                    )
                        ? bottom
                        : 0,
                    decimal.TryParse(
                        form["marginLeftMillimetres"],
                        CultureInfo.InvariantCulture,
                        out var left
                    )
                        ? left
                        : 0
                );
                await using var stream = file.OpenReadStream();
                var result = await catalog.CreateAsync(request, stream, ct);
                return result is null ? TypedResults.Conflict() : TypedResults.Ok(result);
            }
        );
        documents.MapPost(
            "/letterheads/{id:guid}/default",
            async Task<Results<Ok, NotFound>> (
                Guid id,
                LetterheadCatalog catalog,
                CancellationToken ct
            ) => await catalog.SetDefaultAsync(id, ct) ? TypedResults.Ok() : TypedResults.NotFound()
        );
    }
#pragma warning restore MA0051

    private static async Task<
        Results<Ok<EquipmentItemSnapshot>, NotFound, BadRequest<string>>
    > SaveEquipmentImageAsync(
        Guid id,
        HttpRequest request,
        EquipmentCatalog catalog,
        CancellationToken ct
    )
    {
        var form = await request.ReadFormAsync(ct);
        var file = form.Files.GetFile("file");
        if (file is null)
            return TypedResults.BadRequest<string>("An image file is required.");
        await using var stream = file.OpenReadStream();
        using var memory = new MemoryStream();
        await stream.CopyToAsync(memory, ct);
        var item = await catalog.SaveImageAsync(
            id,
            file.FileName,
            file.ContentType,
            memory.ToArray(),
            ct
        );
        return item is null
            ? TypedResults.BadRequest<string>("The image is empty, too large, or unsupported.")
            : TypedResults.Ok(item);
    }

    private static void MapCustomers(RouteGroupBuilder api)
    {
        var customers = api.MapGroup("/customers").WithTags("Customers");
        customers.MapGet(
            "/",
            async (
                string? tag,
                string? folder,
                CustomerDirectory directory,
                CancellationToken ct
            ) => TypedResults.Ok(await directory.ListAsync(ct, tag, folder))
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
        MapCustomerImport(customers);
        MapContactPersons(customers);
        customers.MapGet("/{id:guid}/taxonomy", GetTaxonomyAsync);
        customers.MapPut("/{id:guid}/taxonomy", SetTaxonomyAsync);
    }

    private static async Task<Results<Ok<ContactTaxonomyResponse>, NotFound>> GetTaxonomyAsync(
        Guid id,
        ContactTaxonomyService taxonomy,
        CancellationToken ct
    ) =>
        await taxonomy.GetAsync(id, ct) is { } result
            ? TypedResults.Ok(result)
            : TypedResults.NotFound();

    private static async Task<Results<Ok<ContactTaxonomyResponse>, NotFound>> SetTaxonomyAsync(
        Guid id,
        ContactTaxonomyRequest request,
        ContactTaxonomyService taxonomy,
        CancellationToken ct
    ) =>
        await taxonomy.SetAsync(id, request, ct) is { } result
            ? TypedResults.Ok(result)
            : TypedResults.NotFound();

    private static void MapContactPersons(RouteGroupBuilder customers)
    {
        customers.MapGet(
            "/{id:guid}/contact-persons",
            async (Guid id, ContactPersonService persons, CancellationToken ct) =>
                TypedResults.Ok(await persons.ListAsync(id, ct))
        );
        customers.MapPost(
            "/{id:guid}/contact-persons",
            async Task<Results<Created<ContactPersonResponse>, NotFound, BadRequest<string>>> (
                Guid id,
                CreateContactPersonRequest request,
                ContactPersonService persons,
                CancellationToken ct
            ) =>
            {
                try
                {
                    var person = await persons.AddAsync(id, request, ct);
                    return person is null
                        ? TypedResults.NotFound()
                        : TypedResults.Created(
                            $"/api/customers/{id}/contact-persons/{person.Id}",
                            person
                        );
                }
                catch (ArgumentException exception)
                {
                    return TypedResults.BadRequest(exception.Message);
                }
            }
        );
    }

    private static void MapCustomerImport(RouteGroupBuilder customers)
    {
        customers.MapPost(
            "/import",
            async (IFormFile file, CustomerDirectory directory, CancellationToken ct) =>
            {
                await using var stream = file.OpenReadStream();
                return TypedResults.Ok(await directory.ImportCsvAsync(stream, ct));
            }
        );
    }

    private static void MapContacts(RouteGroupBuilder api)
    {
        var contacts = api.MapGroup("/contacts").WithTags("Contacts");
        contacts.MapGet(
            "/{id:guid}",
            async Task<Results<Ok<ContactResponse>, NotFound>> (
                Guid id,
                CustomerDirectory directory,
                CancellationToken ct
            ) =>
                await directory.GetContactAsync(id, ct) is { } contact
                    ? TypedResults.Ok(contact)
                    : TypedResults.NotFound()
        );
        contacts.MapGet(
            "/",
            async (int? page, int? pageSize, CustomerDirectory directory, CancellationToken ct) =>
                TypedResults.Ok(await directory.ListContactsAsync(page ?? 1, pageSize ?? 20, ct))
        );
        contacts.MapPost(
            "/",
            async (
                RegisterContactRequest request,
                CustomerDirectory directory,
                CancellationToken ct
            ) =>
            {
                var contact = await directory.RegisterContactAsync(request, ct);
                return TypedResults.Created($"/api/contacts/{contact.Id}", contact);
            }
        );
        contacts.MapPut(
            "/{id:guid}",
            async Task<Results<Ok<ContactResponse>, NotFound>> (
                Guid id,
                UpdateContactRequest request,
                CustomerDirectory directory,
                CancellationToken ct
            ) =>
                await directory.UpdateContactAsync(id, request, ct) is { } contact
                    ? TypedResults.Ok(contact)
                    : TypedResults.NotFound()
        );
    }

#pragma warning disable MA0051
    private static void MapProjects(RouteGroupBuilder api)
    {
        var projects = api.MapGroup("/projects").WithTags("Projects");
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
            "/templates",
            async (
                CreateProjectTemplateRequest request,
                ProjectWorkspace workspace,
                CancellationToken ct
            ) => TypedResults.Ok(await workspace.CreateTemplateAsync(request, ct))
        );
        projects.MapPost(
            "/from-template",
            async Task<Results<Created<ProjectResponse>, NotFound>> (
                CreateProjectFromTemplateRequest request,
                ProjectWorkspace workspace,
                CancellationToken ct
            ) =>
                await workspace.CreateFromTemplateAsync(request, ct) is { } project
                    ? TypedResults.Created($"/api/projects/{project.Id}", project)
                    : TypedResults.NotFound()
        );
        projects.MapPut(
            "/{id:guid}/shift",
            async Task<Results<Ok<ProjectResponse>, NotFound>> (
                Guid id,
                ShiftProjectRequest request,
                ProjectWorkspace workspace,
                CancellationToken ct
            ) =>
                await workspace.ShiftAsync(id, request, ct) is { } project
                    ? TypedResults.Ok(project)
                    : TypedResults.NotFound()
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
        projects.MapPost(
            "/{id:guid}/equipment-lines",
            async Task<Results<Created<PlannedEquipmentLineResponse>, NotFound>> (
                Guid id,
                PlanEquipmentRequest request,
                ProjectWorkspace workspace,
                CancellationToken ct
            ) =>
                await workspace.PlanEquipmentAsync(id, request, ct) is { } line
                    ? TypedResults.Created($"/api/projects/{id}/equipment-lines/{line.Id}", line)
                    : TypedResults.NotFound()
        );
        projects.MapPost(
            "/{id:guid}/packing-list",
            async Task<Results<Created<CreatePackingListResponse>, NotFound>> (
                Guid id,
                ProjectWorkspace workspace,
                CancellationToken ct
            ) =>
                await workspace.CreatePackingListAsync(id, ct) is { } list
                    ? TypedResults.Created($"/api/projects/{id}/packing-list", list)
                    : TypedResults.NotFound()
        );
        projects.MapGet(
            "/{id:guid}/packing-list",
            async Task<Results<Ok<CreatePackingListResponse>, NotFound>> (
                Guid id,
                ProjectWorkspace workspace,
                CancellationToken ct
            ) =>
                await workspace.GetPackingListAsync(id, ct) is { } list
                    ? TypedResults.Ok(list)
                    : TypedResults.NotFound()
        );
        projects.MapPost(
            "/{id:guid}/packing-list/{lineId:guid}/pack",
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
            "/{id:guid}/packing-list/{lineId:guid}/book-out",
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
            "/{id:guid}/packing-list/{lineId:guid}/receive",
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
        projects.MapPost(
            "/{id:guid}/packing-list/{lineId:guid}/exception",
            async Task<Results<Ok<PackingLineResponse>, NotFound>> (
                Guid id,
                Guid lineId,
                ReportExceptionRequest request,
                ProjectWorkspace workspace,
                CancellationToken ct
            ) =>
                await workspace.ReportExceptionAsync(id, lineId, request, ct) is { } line
                    ? TypedResults.Ok(line)
                    : TypedResults.NotFound()
        );
        projects.MapPut(
            "/{id:guid}/status",
            async Task<Results<Ok<ProjectResponse>, NotFound>> (
                Guid id,
                UpdateProjectStatusRequest request,
                ProjectWorkspace workspace,
                CancellationToken ct
            ) =>
                await workspace.SetStatusAsync(id, request.Status, ct) is { } project
                    ? TypedResults.Ok(project)
                    : TypedResults.NotFound()
        );
    }
#pragma warning restore MA0051

    private static void MapInvoicing(RouteGroupBuilder api)
    {
        MapInvoiceDrafts(api);
        MapInvoices(api);
    }

#pragma warning disable MA0051
    private static void MapCommercial(RouteGroupBuilder api)
    {
        var quotes = api.MapGroup("/quotes").WithTags("Commercial");
        quotes.MapPost(
            "/projects/{projectId:guid}",
            async Task<Results<Created<QuoteResponse>, NotFound>> (
                Guid projectId,
                QuoteWorkspace workspace,
                CancellationToken ct
            ) =>
                await workspace.GenerateAsync(projectId, ct) is { } quote
                    ? TypedResults.Created($"/api/quotes/{quote.Id}", quote)
                    : TypedResults.NotFound()
        );
        quotes.MapGet(
            "/{id:guid}/document",
            async Task<Results<FileContentHttpResult, NotFound>> (
                Guid id,
                QuoteWorkspace workspace,
                CancellationToken ct
            ) =>
                await workspace.GetDocumentAsync(id, ct) is { } document
                    ? TypedResults.File(document.Content, document.ContentType, document.FileName)
                    : TypedResults.NotFound()
        );
        quotes.MapGet(
            "/{id:guid}",
            async Task<Results<Ok<QuoteResponse>, NotFound>> (
                Guid id,
                QuoteWorkspace workspace,
                CancellationToken ct
            ) =>
                await workspace.GetAsync(id, ct) is { } quote
                    ? TypedResults.Ok(quote)
                    : TypedResults.NotFound()
        );
        quotes.MapPut(
            "/{id:guid}/review",
            async Task<Results<Ok<QuoteResponse>, NotFound>> (
                Guid id,
                UpdateQuoteReviewRequest request,
                QuoteWorkspace workspace,
                CancellationToken ct
            ) =>
                await workspace.UpdateReviewAsync(id, request, ct) is { } quote
                    ? TypedResults.Ok(quote)
                    : TypedResults.NotFound()
        );
        quotes.MapPost(
            "/{id:guid}/accept",
            async Task<Results<Ok<QuoteResponse>, NotFound>> (
                Guid id,
                QuoteWorkspace workspace,
                CancellationToken ct
            ) =>
                await workspace.AcceptAsync(id, ct) is { } quote
                    ? TypedResults.Ok(quote)
                    : TypedResults.NotFound()
        );
        quotes.MapPost(
            "/{id:guid}/sign",
            async Task<Results<Ok<QuoteResponse>, NotFound>> (
                Guid id,
                SignQuoteRequest request,
                QuoteWorkspace workspace,
                CancellationToken ct
            ) =>
                await workspace.SignAsync(id, request, ct) is { } quote
                    ? TypedResults.Ok(quote)
                    : TypedResults.NotFound()
        );
    }

#pragma warning restore MA0051

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
        invoices.MapPost(
            "/{id:guid}/rectifications",
            async Task<Results<Created<InvoiceDraftResponse>, NotFound>> (
                Guid id,
                RectifyInvoiceRequest request,
                InvoicingWorkflow invoicing,
                CancellationToken ct
            ) =>
                await invoicing.CreateRectificationDraftAsync(id, request, ct) is { } draft
                    ? TypedResults.Created($"/api/invoice-drafts/{draft.Id}", draft)
                    : TypedResults.NotFound()
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
