using Raccord.Domain.Billing;

namespace Raccord.Application.Documents;

/// <summary>Renders the human-readable document of an issued invoice.</summary>
public interface IInvoiceDocumentRenderer
{
    /// <summary>Media type of the rendered document, for example <c>application/pdf</c>.</summary>
    string ContentType { get; }

    /// <summary>File extension without the dot.</summary>
    string FileExtension { get; }

    /// <summary>Renders the invoice. The same invoice always produces the same bytes.</summary>
    byte[] Render(IssuedInvoice invoice);

    /// <summary>Renders the invoice using the requested two-letter presentation language.</summary>
    byte[] Render(IssuedInvoice invoice, string language);
}
