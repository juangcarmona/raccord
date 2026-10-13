using Raccord.Domain.Billing;
using Raccord.Domain.Projects;
using Raccord.SharedKernel.Common;
using Raccord.SharedKernel.Parties;
using Raccord.SharedKernel.Pricing;

namespace Raccord.Domain.Tests.Billing;

public sealed class IssuedInvoiceRepresentationTests
{
    private readonly IssuedInvoice invoice = IssueSampleInvoice();

    [Fact]
    public void The_representation_Should_be_deterministic()
    {
        var first = IssuedInvoiceRepresentation.From(invoice);
        var second = IssuedInvoiceRepresentation.From(invoice);

        second.Json.ShouldBe(first.Json);
        second.ContentHash.ShouldBe(first.ContentHash);
        first.ContentHash.ShouldMatch("^[0-9a-f]{64}$");
    }

    [Fact]
    public void Amounts_Should_be_written_as_fixed_point_strings()
    {
        var json = IssuedInvoiceRepresentation.From(invoice).Json;

        json.ShouldContain("\"taxableBase\":\"1280.00\"", Case.Sensitive);
        json.ShouldContain("\"total\":\"1548.80\"", Case.Sensitive);
        json.ShouldContain("\"schema\":\"raccord.issued-invoice/2\"", Case.Sensitive);
    }

    [Fact]
    public void A_stored_representation_Should_restore_the_same_invoice()
    {
        var stored = IssuedInvoiceRepresentation.From(invoice);

        var restored = IssuedInvoiceRepresentation.Restore(stored.Json, stored.ContentHash);

        restored.Id.ShouldBe(invoice.Id);
        restored.Number.ShouldBe(invoice.Number);
        restored.Issuer.ShouldBe(invoice.Issuer);
        restored.Recipient.ShouldBe(invoice.Recipient);
        restored.Totals.Total.ShouldBe(invoice.Totals.Total);
        IssuedInvoiceRepresentation.From(restored).Json.ShouldBe(stored.Json);
    }

    [Fact]
    public void A_tampered_representation_Should_be_rejected()
    {
        var stored = IssuedInvoiceRepresentation.From(invoice);
        var tampered = stored.Json.Replace(
            "\"quantity\":\"2\"",
            "\"quantity\":\"1\"",
            StringComparison.Ordinal
        );

        var error = Should.Throw<BusinessRuleViolationException>(() =>
            IssuedInvoiceRepresentation.Restore(tampered, stored.ContentHash)
        );

        error.Code.ShouldBe("issued-invoice.integrity-violation");
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
