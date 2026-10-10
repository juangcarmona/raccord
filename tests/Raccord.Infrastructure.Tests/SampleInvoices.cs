using Raccord.Domain.Billing;
using Raccord.Domain.Parties;
using Raccord.Domain.Pricing;
using Raccord.Domain.Projects;

namespace Raccord.Infrastructure.Tests;

internal static class SampleInvoices
{
    public static IssuedInvoice Issue(int lineCount = 2)
    {
        var project = Project.Create("Rodaje Peñíscola", Guid.CreateVersion7(), new DateOnly(2026, 5, 4), new DateOnly(2026, 5, 8));
        var lines = Enumerable.Range(1, lineCount)
            .Select(index => project.AddBillableLine(
                BillableLineKind.Equipment,
                index == 1 ? "Kit de cámara de cine – días 1–2" : $"Line {index}",
                LinePricing.Create(index == 1 ? 2m : 1m, index == 1 ? 450m : 380m, 0m, VatRate.Of(21m))))
            .ToList();
        var draft = InvoiceDraft.CreateFromProject(project, [.. lines.Select(line => line.Id)], "F", DateTimeOffset.UnixEpoch);
        var issuer = new LegalParty("Fotograma Rental S.L.", "B00000000", new PostalAddress("Calle Mayor 1", "28013", "Madrid", "Madrid", "ES"));
        var recipient = new LegalParty("Productora Ejemplo S.A.", "A00000000", new PostalAddress("Avenida del Puerto 20", "46023", "Valencia", "Valencia", "ES"));
        return draft.Finalize(issuer, recipient, InvoiceNumberSequence.Start("F", 2026), new DateOnly(2026, 5, 11), new DateTimeOffset(2026, 5, 11, 9, 30, 0, TimeSpan.Zero));
    }
}
