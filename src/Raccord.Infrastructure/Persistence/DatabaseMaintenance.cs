using Microsoft.EntityFrameworkCore;
using Raccord.Domain.Billing;
using Raccord.Domain.Customers;
using Raccord.Domain.Organization;
using Raccord.Domain.Parties;
using Raccord.Domain.Pricing;
using Raccord.Domain.Projects;

namespace Raccord.Infrastructure.Persistence;

/// <summary>Applies migrations, reference data and (on request) fictional demo data.</summary>
public sealed class DatabaseMaintenance(RaccordDbContext db)
{
    /// <summary>Applies pending migrations and ensures the invoice series every installation needs.</summary>
    public async Task MigrateAsync(CancellationToken cancellationToken)
    {
        await db.Database.MigrateAsync(cancellationToken).ConfigureAwait(false);

        if (!await db.InvoiceSeries.AnyAsync(cancellationToken).ConfigureAwait(false))
        {
            db.InvoiceSeries.Add(InvoiceSeries.Define("F", "Facturas ordinarias", isDefault: true));
            db.InvoiceSeries.Add(InvoiceSeries.Define("R", "Facturas rectificativas (reservada)", isDefault: false));
            await db.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }
    }

    /// <summary>
    /// Adds a fictional issuer, customers and projects when the database has no customers.
    /// Names and tax IDs are invented and must never be replaced with real data in the repository.
    /// </summary>
    public async Task SeedDemoDataAsync(CancellationToken cancellationToken)
    {
        if (await db.Customers.AnyAsync(cancellationToken).ConfigureAwait(false))
        {
            return;
        }

        if (!await db.IssuerProfiles.AnyAsync(cancellationToken).ConfigureAwait(false))
        {
            db.IssuerProfiles.Add(IssuerProfile.Define("Fotograma Rental Demo S.L.", "B00000000", new PostalAddress("Calle del Plató 12", "28045", "Madrid", "Madrid", "ES")));
        }

        var producer = Customer.Register("Productora Ejemplo S.A.", "A00000000", new PostalAddress("Avenida del Puerto 20", "46023", "Valencia", "Valencia", "ES"), "facturacion@productora.example");
        var agency = Customer.Register("Agencia Creativa Demo S.L.", null, null, "admin@agencia.example");
        db.Customers.AddRange(producer, agency);

        var shoot = Project.Create("Rodaje campaña primavera", producer.Id, new DateOnly(2026, 9, 14), new DateOnly(2026, 9, 18));
        shoot.AddBillableLine(BillableLineKind.Equipment, "Kit de cámara de cine con ópticas, 5 jornadas", LinePricing.Create(5m, 450m, 10m, VatRate.Of(21m)));
        shoot.AddBillableLine(BillableLineKind.Equipment, "Paquete de iluminación LED", LinePricing.Create(5m, 280m, 10m, VatRate.Of(21m)));
        shoot.AddBillableLine(BillableLineKind.Labour, "Gaffer, jornada de 10 h", LinePricing.Create(5m, 380m, 0m, VatRate.Of(21m)));
        shoot.AddBillableLine(BillableLineKind.Transport, "Furgoneta con conductor", LinePricing.Create(2m, 220m, 0m, VatRate.Of(21m)));
        shoot.AddBillableLine(BillableLineKind.Consumable, "Gelatinas y difusores", LinePricing.Create(1m, 64.35m, 0m, VatRate.Of(21m)));

        var stream = Project.Create("Streaming evento corporativo", agency.Id, new DateOnly(2026, 10, 1), new DateOnly(2026, 10, 1));
        stream.AddBillableLine(BillableLineKind.Service, "Realización multicámara y streaming", LinePricing.Create(1m, 2400m, 0m, VatRate.Of(21m)));
        stream.AddBillableLine(BillableLineKind.Labour, "Técnico de sonido, 8 h", LinePricing.Create(8m, 42.5m, 0m, VatRate.Of(21m)));

        db.Projects.AddRange(shoot, stream);
        await db.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }
}
