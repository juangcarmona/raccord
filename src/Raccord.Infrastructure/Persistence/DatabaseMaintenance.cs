using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Raccord.Domain.Billing;
using Raccord.Domain.Projects;
using Raccord.Modules.Contacts.Contracts;
using Raccord.Modules.Workspace.Contracts;
using Raccord.SharedKernel.Parties;
using Raccord.SharedKernel.Pricing;

namespace Raccord.Infrastructure.Persistence;

/// <summary>Applies migrations, reference data and (on request) fictional demo data.</summary>
public sealed class DatabaseMaintenance(
    RaccordDbContext db,
    IPartyDirectory parties,
    IPartyWriter partyWriter,
    IWorkspaceStore workspace,
    UserManager<RaccordUser> users,
    InstallationSetup installation
)
{
    /// <summary>
    /// Adds a fictional issuer, customers and projects when the database has no customers.
    /// Names and tax IDs are invented and must never be replaced with real data in the repository.
    /// </summary>
    public async Task SeedDemoDataAsync(CancellationToken cancellationToken)
    {
        if (!await users.Users.AnyAsync(cancellationToken).ConfigureAwait(false))
        {
            var result = await installation
                .CreateFirstAdministratorAsync(
                    "admin@example.com",
                    "DemoPassword123!",
                    cancellationToken
                )
                .ConfigureAwait(false);
            if (result.Kind is not InstallationSetup.SetupResultKind.Created)
            {
                throw new InvalidOperationException(string.Join("; ", result.Errors));
            }
        }

        if ((await parties.ListAsync(cancellationToken).ConfigureAwait(false)).Count > 0)
        {
            return;
        }

        if (await workspace.FindIssuerAsync(cancellationToken).ConfigureAwait(false) is null)
        {
            workspace.AddIssuer(
                new IssuerProfileSnapshot(
                    Guid.CreateVersion7(),
                    "Fotograma Rental Demo S.L.",
                    "B00000000",
                    new PostalAddress("Calle del Plató 12", "28045", "Madrid", "Madrid", "ES")
                )
            );
        }

        var producer = new PartySnapshot(
            Guid.CreateVersion7(),
            "Productora Ejemplo S.A.",
            "A00000000",
            new PostalAddress("Avenida del Puerto 20", "46023", "Valencia", "Valencia", "ES"),
            "facturacion@productora.example"
        );
        var agency = new PartySnapshot(
            Guid.CreateVersion7(),
            "Agencia Creativa Demo S.L.",
            null,
            null,
            "admin@agencia.example"
        );
        partyWriter.Add(producer);
        partyWriter.Add(agency);

        db.Projects.AddRange(
            CreateShootDemoProject(producer.Id),
            CreateStreamingDemoProject(agency.Id)
        );
        await db.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    private static Project CreateShootDemoProject(Guid producerId)
    {
        var shoot = Project.Create(
            "Rodaje campaña primavera",
            producerId,
            new DateOnly(2026, 9, 14),
            new DateOnly(2026, 9, 18)
        );
        shoot.AddBillableLine(
            BillableLineKind.Equipment,
            "Kit de cámara de cine con ópticas, 5 jornadas",
            LinePricing.Create(5m, 450m, 10m, VatRate.Of(21m))
        );
        shoot.AddBillableLine(
            BillableLineKind.Equipment,
            "Paquete de iluminación LED",
            LinePricing.Create(5m, 280m, 10m, VatRate.Of(21m))
        );
        shoot.AddBillableLine(
            BillableLineKind.Labour,
            "Gaffer, jornada de 10 h",
            LinePricing.Create(5m, 380m, 0m, VatRate.Of(21m))
        );
        shoot.AddBillableLine(
            BillableLineKind.Transport,
            "Furgoneta con conductor",
            LinePricing.Create(2m, 220m, 0m, VatRate.Of(21m))
        );
        shoot.AddBillableLine(
            BillableLineKind.Consumable,
            "Gelatinas y difusores",
            LinePricing.Create(1m, 64.35m, 0m, VatRate.Of(21m))
        );
        return shoot;
    }

    private static Project CreateStreamingDemoProject(Guid agencyId)
    {
        var stream = Project.Create(
            "Streaming evento corporativo",
            agencyId,
            new DateOnly(2026, 10, 1),
            new DateOnly(2026, 10, 1)
        );
        stream.AddBillableLine(
            BillableLineKind.Service,
            "Realización multicámara y streaming",
            LinePricing.Create(1m, 2400m, 0m, VatRate.Of(21m))
        );
        stream.AddBillableLine(
            BillableLineKind.Labour,
            "Técnico de sonido, 8 h",
            LinePricing.Create(8m, 42.5m, 0m, VatRate.Of(21m))
        );
        return stream;
    }

    /// <summary>Applies pending migrations and ensures the invoice series every installation needs.</summary>
    public async Task MigrateAsync(CancellationToken cancellationToken)
    {
        await db.Database.MigrateAsync(cancellationToken).ConfigureAwait(false);

        if (!(await workspace.ListAsync(cancellationToken).ConfigureAwait(false)).Any())
        {
            workspace.AddSeries(new InvoiceSeriesSnapshot("F", "Facturas ordinarias", true));
            workspace.AddSeries(
                new InvoiceSeriesSnapshot("R", "Facturas rectificativas (reservada)", false)
            );
            await db.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }
    }
}
