using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Raccord.Contracts.Invoicing;
using Raccord.Contracts.Projects;
using Raccord.Infrastructure.Persistence;
using Raccord.Web.Tests.Infrastructure;

namespace Raccord.Web.Tests;

public sealed class VatCatalogTests(PostgresContainer postgres) : IAsyncLifetime
{
    private RaccordApi api = null!;
    private HttpClient client = null!;

    public async ValueTask InitializeAsync()
    {
        api = await RaccordApi.StartAsync(postgres);
        client = api.CreateClient();
    }

    public async ValueTask DisposeAsync()
    {
        client.Dispose();
        await api.DisposeAsync();
    }

    [Fact]
    public async Task Fresh_install_Should_ExposeSourceDatedDefaultVatScheme()
    {
        var classes = await client.GetFromJsonAsync<List<VatClassResponse>>(
            "/api/invoicing/vat-classes"
        );
        var schemes = await client.GetFromJsonAsync<List<VatSchemeResponse>>(
            "/api/invoicing/vat-schemes"
        );

        classes.ShouldNotBeNull();
        classes.ShouldContain(item =>
            string.Equals(item.Name, "General", StringComparison.Ordinal)
        );
        var general = classes.Single(item =>
            string.Equals(item.Name, "General", StringComparison.Ordinal)
        );
        schemes.ShouldNotBeNull();
        var scheme = schemes.ShouldHaveSingleItem();
        scheme.IsDefault.ShouldBeTrue();
        scheme.Kind.ShouldBe("per-class");
        scheme.ClassRates.ShouldContain(rate =>
            rate.VatClassId == general.Id && rate.Percentage == 21m
        );
        scheme.Territory.ShouldBe("ES");
        scheme.SourceUrl.ShouldStartWith("https://sede.agenciatributaria.gob.es/");
        scheme.VerifiedOn.ShouldBe(new DateOnly(2026, 10, 21));
    }

    [Fact]
    public async Task Bootstrap_Should_BeIdempotentAndPreserveAdministratorChanges()
    {
        await MigrateAgainAsync();
        (await api.QueryScalarAsync<long>("select count(*) from invoicing.vat_schemes")).ShouldBe(
            1
        );
        (
            await api.QueryScalarAsync<long>(
                "select count(*) from compliance.audit_events where action = 'vat-catalog.initialized'"
            )
        ).ShouldBe(1);
        await api.ExecuteAsync("update invoicing.vat_schemes set name = 'Administrator choice'");

        await MigrateAgainAsync();

        (
            await api.QueryScalarAsync<string>(
                "select name from invoicing.vat_schemes where is_default"
            )
        ).ShouldBe("Administrator choice");
    }

    [Fact]
    public async Task Catalog_mutations_Should_CreateAndAuditStableSchemeIdentity()
    {
        var classResponse = await client.PostAsJsonAsync(
            "/api/invoicing/vat-classes",
            new CreateVatClassRequest("Reduced")
        );
        classResponse.StatusCode.ShouldBe(HttpStatusCode.Created);
        var vatClass = await classResponse.Content.ReadFromJsonAsync<VatClassResponse>();
        vatClass.ShouldNotBeNull();

        var schemeResponse = await client.PostAsJsonAsync(
            "/api/invoicing/vat-schemes",
            new CreateVatSchemeRequest(
                "Reduced rate",
                "per-class",
                null,
                [new VatClassRateRequest(vatClass.Id, 10m)]
            )
        );
        schemeResponse.StatusCode.ShouldBe(HttpStatusCode.Created);
        var scheme = await schemeResponse.Content.ReadFromJsonAsync<VatSchemeResponse>();
        scheme.ShouldNotBeNull();
        scheme.ClassRates.ShouldContain(rate =>
            rate.VatClassId == vatClass.Id && rate.Percentage == 10m
        );

        var makeDefault = await client.PutAsync(
            $"/api/invoicing/vat-schemes/{scheme.Id}/default",
            content: null
        );
        makeDefault.StatusCode.ShouldBe(HttpStatusCode.OK);
        (
            await api.QueryScalarAsync<long>(
                $"select count(*) from invoicing.vat_schemes where id = '{scheme.Id:D}' and is_default"
            )
        ).ShouldBe(1);
        (
            await api.QueryScalarAsync<long>(
                $"select count(*) from compliance.audit_events where entity_id = '{scheme.Id:D}' and action = 'vat-scheme.default-set'"
            )
        ).ShouldBe(1);
        (
            await api.QueryScalarAsync<string>(
                $"select details->>'newDefaultSchemeId' from compliance.audit_events where entity_id = '{scheme.Id:D}' and action = 'vat-scheme.default-set'"
            )
        ).ShouldBe(scheme.Id.ToString("D"));
        (
            await api.QueryScalarAsync<string>(
                $"select details->>'rates' from compliance.audit_events where entity_id = '{scheme.Id:D}' and action = 'vat-scheme.created'"
            )
        ).ShouldBe($"{vatClass.Id:D}=10");
    }

    [Fact]
    public async Task Unused_scheme_Should_AllowRateChangesWithoutChangingItsIdentity()
    {
        var vatClass = await CreateClassAsync();
        var scheme = await CreatePerClassSchemeAsync(vatClass.Id, 10m);

        var response = await client.PutAsJsonAsync(
            $"/api/invoicing/vat-schemes/{scheme.Id}",
            new UpdateVatSchemeRequest(
                "Reduced rate revised",
                null,
                [new VatClassRateRequest(vatClass.Id, 12m)]
            )
        );

        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        var updated = await response.Content.ReadFromJsonAsync<VatSchemeResponse>();
        updated.ShouldNotBeNull();
        updated.Id.ShouldBe(scheme.Id);
        updated.Name.ShouldBe("Reduced rate revised");
        updated.ClassRates.ShouldContain(rate => rate.Percentage == 12m);
        (
            await api.QueryScalarAsync<long>(
                $"select count(*) from compliance.audit_events where entity_id = '{scheme.Id:D}' and action = 'vat-scheme.updated'"
            )
        ).ShouldBe(1);
        (
            await api.QueryScalarAsync<string>(
                $"select details->>'previousRates' from compliance.audit_events where entity_id = '{scheme.Id:D}' and action = 'vat-scheme.updated'"
            )
        ).ShouldBe($"{vatClass.Id:D}=10");
        (
            await api.QueryScalarAsync<string>(
                $"select details->>'newRates' from compliance.audit_events where entity_id = '{scheme.Id:D}' and action = 'vat-scheme.updated'"
            )
        ).ShouldBe($"{vatClass.Id:D}=12");
    }

    [Fact]
    public async Task Class_rename_Should_PreserveIdentityAndAuditBeforeAndAfterNames()
    {
        var vatClass = await CreateClassAsync();

        var response = await client.PutAsJsonAsync(
            $"/api/invoicing/vat-classes/{vatClass.Id}",
            new RenameVatClassRequest("Services")
        );

        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        var renamed = await response.Content.ReadFromJsonAsync<VatClassResponse>();
        renamed.ShouldNotBeNull();
        renamed.Id.ShouldBe(vatClass.Id);
        renamed.Name.ShouldBe("Services");
        (
            await api.QueryScalarAsync<string>(
                $"select details->>'previousName' from compliance.audit_events where entity_id = '{vatClass.Id:D}' and action = 'vat-class.renamed'"
            )
        ).ShouldBe("Reduced");
        (
            await api.QueryScalarAsync<string>(
                $"select details->>'newName' from compliance.audit_events where entity_id = '{vatClass.Id:D}' and action = 'vat-class.renamed'"
            )
        ).ShouldBe("Services");
    }

    [Fact]
    public async Task Saved_project_line_Should_RetainSchemeIdentityAndFreezeItsRate()
    {
        var vatClass = await CreateClassAsync();
        var scheme = await CreatePerClassSchemeAsync(vatClass.Id, 10m);
        var project = await RaccordApi.ArrangeProjectAsync(
            client,
            lines:
            [
                new AddBillableLineRequest(
                    "service",
                    "Translation services",
                    1m,
                    100m,
                    0m,
                    vatClass.Id,
                    scheme.Id
                ),
            ]
        );

        var line = project.BillableLines.ShouldHaveSingleItem();
        line.VatClassId.ShouldBe(vatClass.Id);
        line.VatSchemeId.ShouldBe(scheme.Id);
        line.VatRate.ShouldBe(10m);
        var editUsed = await client.PutAsJsonAsync(
            $"/api/invoicing/vat-schemes/{scheme.Id}",
            new UpdateVatSchemeRequest(
                "Reduced rate",
                null,
                [new VatClassRateRequest(vatClass.Id, 12m)]
            )
        );
        editUsed.StatusCode.ShouldBe(HttpStatusCode.UnprocessableEntity);
        (
            await api.QueryScalarAsync<long>(
                $"select count(*) from compliance.audit_events where entity_id = '{scheme.Id:D}' and action = 'vat-scheme.updated'"
            )
        ).ShouldBe(0);
    }

    [Fact]
    public async Task Concurrent_first_useAndRateUpdate_Should_KeepTheSavedRateConsistent()
    {
        var vatClass = await CreateClassAsync();
        var scheme = await CreatePerClassSchemeAsync(vatClass.Id, 10m);
        var project = await RaccordApi.ArrangeProjectAsync(client);
        var updateTask = UpdateSchemeAsync(scheme.Id, scheme.Name, vatClass.Id, 12m);
        var addLineTask = AddLineAsync(project.Id, vatClass.Id, scheme.Id);

        await Task.WhenAll(updateTask, addLineTask);

        var updated = await updateTask;
        var added = await addLineTask;
        updated.StatusCode.ShouldBeOneOf(
            HttpStatusCode.OK,
            HttpStatusCode.Conflict,
            HttpStatusCode.UnprocessableEntity
        );
        if (added.StatusCode is HttpStatusCode.Conflict)
        {
            added = await AddLineAsync(project.Id, vatClass.Id, scheme.Id);
        }
        added.StatusCode.ShouldBe(HttpStatusCode.Created);
        var reloaded = await client.GetFromJsonAsync<ProjectResponse>(
            $"/api/projects/{project.Id}"
        );
        var line = reloaded!.BillableLines.ShouldHaveSingleItem();
        line.VatSchemeId.ShouldBe(scheme.Id);
        line.VatClassId.ShouldBe(vatClass.Id);
        line.VatRate.ShouldBeOneOf(10m, 12m);

        var postUseEdit = await UpdateSchemeAsync(scheme.Id, scheme.Name, vatClass.Id, 15m);
        postUseEdit.StatusCode.ShouldBe(HttpStatusCode.UnprocessableEntity);
    }

    private Task<HttpResponseMessage> AddLineAsync(Guid projectId, Guid classId, Guid schemeId) =>
        client.PostAsJsonAsync(
            $"/api/projects/{projectId}/billable-lines",
            new AddBillableLineRequest(
                "service",
                "Race condition service",
                1m,
                100m,
                0m,
                classId,
                schemeId
            )
        );

    private Task<HttpResponseMessage> UpdateSchemeAsync(
        Guid schemeId,
        string name,
        Guid classId,
        decimal rate
    ) =>
        client.PutAsJsonAsync(
            $"/api/invoicing/vat-schemes/{schemeId}",
            new UpdateVatSchemeRequest(name, null, [new VatClassRateRequest(classId, rate)])
        );

    private async Task<VatClassResponse> CreateClassAsync() =>
        await (
            await client.PostAsJsonAsync(
                "/api/invoicing/vat-classes",
                new CreateVatClassRequest("Reduced")
            )
        ).Content.ReadFromJsonAsync<VatClassResponse>()
        ?? throw new InvalidOperationException("The VAT class response was empty.");

    private async Task<VatSchemeResponse> CreatePerClassSchemeAsync(Guid classId, decimal rate) =>
        await (
            await client.PostAsJsonAsync(
                "/api/invoicing/vat-schemes",
                new CreateVatSchemeRequest(
                    "Reduced rate",
                    "per-class",
                    null,
                    [new VatClassRateRequest(classId, rate)]
                )
            )
        ).Content.ReadFromJsonAsync<VatSchemeResponse>()
        ?? throw new InvalidOperationException("The VAT scheme response was empty.");

    private async Task MigrateAgainAsync()
    {
        await using var scope = api.Services.CreateAsyncScope();
        await scope
            .ServiceProvider.GetRequiredService<DatabaseMaintenance>()
            .MigrateAsync(CancellationToken.None);
    }

    [Fact]
    public async Task Invalid_class_mapping_Should_ReturnValidationProblemWithoutAuditEvent()
    {
        var response = await client.PostAsJsonAsync(
            "/api/invoicing/vat-schemes",
            new CreateVatSchemeRequest(
                "Unmapped",
                "per-class",
                null,
                [new VatClassRateRequest(Guid.NewGuid(), 10m)]
            )
        );

        response.StatusCode.ShouldBe(HttpStatusCode.UnprocessableEntity);
        (
            await api.QueryScalarAsync<long>(
                "select count(*) from compliance.audit_events where action = 'vat-scheme.created'"
            )
        ).ShouldBe(0);
    }

    [Fact]
    public async Task New_project_line_Should_RejectMissingStableVatReferences()
    {
        var project = await RaccordApi.ArrangeProjectAsync(client);
        var response = await client.PostAsJsonAsync(
            $"/api/projects/{project.Id}/billable-lines",
            new
            {
                kind = "service",
                description = "Unclassified service",
                quantity = 1m,
                unitPrice = 100m,
                discountPercent = 0m,
            }
        );

        response.StatusCode.ShouldBe(HttpStatusCode.UnprocessableEntity);
        using var problem = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        problem
            .RootElement.GetProperty("code")
            .GetString()
            .ShouldBe("invoicing.vat-reference-incomplete");
        (
            await api.QueryScalarAsync<long>(
                $"select count(*) from projects.billable_lines where project_id = '{project.Id:D}'"
            )
        ).ShouldBe(0);
    }

    [Fact]
    public async Task Catalog_mutations_Should_RequireInvoicingEditPermission()
    {
        client.DefaultRequestHeaders.Add("X-Test-NoPermissions", "true");

        var list = await client.GetAsync("/api/invoicing/vat-schemes");
        var create = await client.PostAsJsonAsync(
            "/api/invoicing/vat-classes",
            new CreateVatClassRequest("Restricted")
        );

        list.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
        create.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }
}
