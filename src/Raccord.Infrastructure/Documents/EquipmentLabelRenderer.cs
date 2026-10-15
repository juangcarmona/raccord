using Raccord.Infrastructure.Documents.Pdf;
using Raccord.Modules.Equipment.Contracts;

namespace Raccord.Infrastructure.Documents;

/// <summary>Renders the printable PDF representation of one equipment label.</summary>
public static class EquipmentLabelRenderer
{
    /// <summary>Creates a deterministic PDF containing the equipment identity and stock details.</summary>
    public static byte[] Render(EquipmentItemSnapshot item)
    {
        ArgumentNullException.ThrowIfNull(item);

        var page = new PdfPage();
        page.Band(40, 720, 515, 72);
        page.Text(55, 755, "EQUIPMENT", 11, bold: true);
        page.Text(55, 730, item.Name, 18, bold: true);
        page.Text(55, 690, $"Code: {item.Code}", 14, bold: true);
        page.Text(55, 665, $"Type: {item.Type}", 11);
        page.Text(55, 642, $"Stock: {item.StockQuantity}", 11);
        page.FillRectangle(390, 610, 130, 130, gray: 0.9);
        page.Text(410, 672, item.Code, 12, bold: true);
        page.Text(55, 600, "Scan code", 9);

        return PdfDocumentWriter.Write([page], $"Equipment label {item.Code}");
    }
}
