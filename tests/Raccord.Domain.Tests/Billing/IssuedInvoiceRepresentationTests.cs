using Raccord.Domain.Billing;
using Raccord.Domain.Common;
using Raccord.Domain.Parties;
using Raccord.Domain.Pricing;
using Raccord.Domain.Projects;

namespace Raccord.Domain.Tests.Billing;

public sealed class IssuedInvoiceRepresentationTests
{
    private readonly IssuedInvoice invoice = IssueSampleInvoice();

    [Fact]
    public void The_representation_is_deterministic()
    {
        var first = IssuedInvoiceRepresentation.From(invoice);
        var second = IssuedInvoiceRepresentation.From(invoice);

        Assert.Equal(first.Json, second.Json);
        Assert.Equal(first.ContentHash, second.ContentHash);
        Assert.Matches("^[0-9a-f]{64}$", first.ContentHash);
    }

    [Fact]
    public void Amounts_are_written_as_fixed_point_strings()
    {
        var json = IssuedInvoiceRepresentation.From(invoice).Json;

        Assert.Contains("\"taxableBase\":\"1280.00\"", json, StringComparison.Ordinal);
        Assert.Contains("\"total\":\"1548.80\"", json, StringComparison.Ordinal);
        Assert.Contains("\"schema\":\"raccord.issued-invoice/1\"", json, StringComparison.Ordinal);
    }

    [Fact]
    public void A_stored_representation_restores_the_same_invoice()
    {
        var stored = IssuedInvoiceRepresentation.From(invoice);

        var restored = IssuedInvoiceRepresentation.Restore(stored.Json, stored.ContentHash);

        Assert.Equal(invoice.Id, restored.Id);
        Assert.Equal(invoice.Number, restored.Number);
        Assert.Equal(invoice.Issuer, restored.Issuer);
        Assert.Equal(invoice.Recipient, restored.Recipient);
        Assert.Equal(invoice.Totals.Total, restored.Totals.Total);
        Assert.Equal(stored.Json, IssuedInvoiceRepresentation.From(restored).Json);
    }

    [Fact]
    public void A_tampered_representation_is_rejected()
    {
        var stored = IssuedInvoiceRepresentation.From(invoice);
        var tampered = stored.Json.Replace(
            "\"quantity\":\"2\"",
            "\"quantity\":\"1\"",
            StringComparison.Ordinal
        );

        var error = Assert.Throws<BusinessRuleViolationException>(() =>
            IssuedInvoiceRepresentation.Restore(tampered, stored.ContentHash)
        );

        Assert.Equal("issued-invoice.integrity-violation", error.Code);
    }

    private static IssuedInvoice IssueSampleInvoice()
    {
        var project = Project.Create(
            "Spring campaign shoot",
            Guid.CreateVersion7(),
            new DateOnly(2026, 5, 4),
            new DateOnly(2026, 5, 8)
        );
        var camera = project.AddBillableLine(
            BillableLineKind.Equipment,
            "Cinema camera kit – días 1–2",
            LinePricing.Create(2m, 450m, 0m, VatRate.Of(21m))
        );
        var gaffer = project.AddBillableLine(
            BillableLineKind.Labour,
            "Gaffer, 10h day",
            LinePricing.Create(1m, 380m, 0m, VatRate.Of(21m))
        );
        var draft = InvoiceDraft.CreateFromProject(
            project,
            [camera.Id, gaffer.Id],
            "F",
            DateTimeOffset.UnixEpoch
        );
        var issuer = new LegalParty(
            "Fotograma Rental S.L.",
            "B00000000",
            new PostalAddress("Calle Mayor 1", "28013", "Madrid", "Madrid", "ES")
        );
        var recipient = new LegalParty(
            "Productora Ejemplo S.A.",
            "A00000000",
            new PostalAddress("Avenida del Puerto 20", "46023", "Valencia", "Valencia", "ES")
        );
        return draft.Finalize(
            issuer,
            recipient,
            InvoiceNumberSequence.Start("F", 2026),
            new DateOnly(2026, 5, 11),
            new DateTimeOffset(2026, 5, 11, 9, 30, 0, TimeSpan.Zero)
        );
    }
}
