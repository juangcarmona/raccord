using Microsoft.AspNetCore.Http.HttpResults;
using Raccord.Application.Auditing;
using Raccord.Application.Invoicing;
using Raccord.Application.Organization;
using Raccord.Application.Projects;
using Raccord.Contracts.Auditing;
using Raccord.Contracts.Contacts;
using Raccord.Contracts.Customers;
using Raccord.Contracts.Invoicing;
using Raccord.Contracts.Organization;
using Raccord.Contracts.Projects;
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

    private static void MapEquipment(RouteGroupBuilder api)
    {
        var equipment = api.MapGroup("/equipment").WithTags("Equipment");
        equipment.MapGet(
            "/items",
            async (int? page, int? pageSize, EquipmentCatalog catalog, CancellationToken ct) =>
                TypedResults.Ok(await catalog.ListAsync(page ?? 1, pageSize ?? 20, ct))
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
    }

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
