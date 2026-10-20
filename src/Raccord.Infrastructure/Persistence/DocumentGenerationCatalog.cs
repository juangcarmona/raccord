using System.Text.Encodings.Web;
using Microsoft.EntityFrameworkCore;
using Raccord.Contracts.Documents;
#pragma warning disable CS1591
namespace Raccord.Infrastructure.Persistence;

public sealed class DocumentGenerationCatalog(RaccordDbContext db)
{
    public async Task<GeneratedProjectDocumentResponse?> GenerateProjectAsync(
        Guid id,
        string audience,
        CancellationToken ct
    )
    {
        var showInternally = audience.Equals("internal", StringComparison.OrdinalIgnoreCase);
        var showExternally = audience.Equals("external", StringComparison.OrdinalIgnoreCase);
        if (!showInternally && !showExternally)
            throw new Raccord.SharedKernel.Common.BusinessRuleViolationException(
                "documents.audience-invalid",
                "Document audience must be internal or external."
            );
        var project = await db
            .Projects.AsNoTracking()
            .Include(item => item.TimePeriods)
            .FirstOrDefaultAsync(item => item.Id == id, ct)
            .ConfigureAwait(false);
        if (project is null)
            return null;
        Func<string, string> encode = HtmlEncoder.Default.Encode;
        var periods = project
            .TimePeriods.Where(period =>
                showInternally ? period.ShowOnInternalDocuments : period.ShowOnExternalDocuments
            )
            .Select(period =>
                $"<li>{encode(period.Name)}{(period.Start is { } start && period.End is { } end ? $" ({start:yyyy-MM-dd} – {end:yyyy-MM-dd})" : string.Empty)}</li>"
            );
        var html =
            $"<article><h1>{encode(project.Name)}</h1><p>Project period: {project.StartDate:yyyy-MM-dd} – {project.EndDate:yyyy-MM-dd}</p><h2>Schedule</h2><ul>{string.Join(string.Empty, periods)}</ul><h2>Equipment</h2><ul>{string.Join(string.Empty, project.PackingLines.Select(line => $"<li>{line.Quantity} × {line.EquipmentId} ({encode(line.Role.ToString())})</li>"))}</ul></article>";
        return new(project.Id, "project", $"project-{project.Id}.html", "text/html", html);
    }

    public async Task<EquipmentLabelResponse?> GenerateLabelAsync(Guid id, CancellationToken ct)
    {
        var item = await db
            .EquipmentItems.AsNoTracking()
            .FirstOrDefaultAsync(candidate => candidate.Id == id, ct)
            .ConfigureAwait(false);
        if (item is null)
            return null;
        Func<string, string> encode = HtmlEncoder.Default.Encode;
        var html =
            $"<label><strong>{encode(item.Code)}</strong><span>{encode(item.Name)}</span><span>Scan: {encode(item.ScanCode ?? item.Code)}</span></label>";
        return new(
            item.Id,
            item.Code,
            item.Name,
            item.ScanCode,
            $"equipment-{item.Code}-label.html",
            "text/html",
            html
        );
    }
}
#pragma warning restore CS1591
