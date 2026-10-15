using System.Globalization;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Raccord.Contracts.Crew;
using Raccord.SharedKernel.Common;

namespace Raccord.Infrastructure.Persistence;

#pragma warning disable CS1591, MA0004
public sealed class CrewHoursCatalog(RaccordDbContext db)
{
    public async Task<CrewHoursResponse> RegisterAsync(
        RegisterCrewHoursRequest request,
        CancellationToken ct
    )
    {
        if (request.Hours <= 0 || request.Hours > 24)
            throw new BusinessRuleViolationException(
                "crew.hours-invalid",
                "Hours must be greater than zero and no more than 24."
            );
        var row = new CrewHoursRow
        {
            Id = Guid.CreateVersion7(),
            CrewMemberId = request.CrewMemberId,
            WorkDate = request.WorkDate,
            Hours = request.Hours,
            Source = request.Source,
            FunctionId = request.FunctionId,
            Remark = Clean(request.Remark),
        };
        db.CrewHours.Add(row);
        await db.SaveChangesAsync(ct);
        return ToResponse(row);
    }

    public async Task<CrewHoursResponse?> ApproveAsync(
        Guid id,
        ApproveCrewHoursRequest request,
        CancellationToken ct
    )
    {
        var row = await db.CrewHours.SingleOrDefaultAsync(item => item.Id == id, ct);
        if (row is null)
            return null;
        row.Status = request.Status;
        await db.SaveChangesAsync(ct);
        return ToResponse(row);
    }

    public async Task<IReadOnlyList<CrewHoursResponse>> ListAsync(
        DateOnly? from,
        DateOnly? to,
        CancellationToken ct
    ) =>
        (
            await db
                .CrewHours.Where(row =>
                    (!from.HasValue || row.WorkDate >= from) && (!to.HasValue || row.WorkDate <= to)
                )
                .OrderBy(row => row.WorkDate)
                .ToListAsync(ct)
        )
            .Select(ToResponse)
            .ToArray();

    public async Task<string> ExportAsync(DateOnly from, DateOnly to, CancellationToken ct)
    {
        var rows = await db
            .CrewHours.Where(row =>
                row.WorkDate >= from && row.WorkDate <= to && row.Status == CrewHourStatus.Approved
            )
            .OrderBy(row => row.WorkDate)
            .ThenBy(row => row.CrewMemberId)
            .ToListAsync(ct);
        var csv = new StringBuilder("crew_member_id,work_date,hours,source,function_id,remark\n");
        foreach (var row in rows)
            csv.Append(row.CrewMemberId)
                .Append(',')
                .Append(row.WorkDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture))
                .Append(',')
                .Append(row.Hours.ToString(CultureInfo.InvariantCulture))
                .Append(',')
                .Append(row.Source)
                .Append(',')
                .Append(row.FunctionId)
                .Append(',')
                .AppendLine(Escape(row.Remark));
        return csv.ToString();
    }

    private static string Escape(string? value) =>
        string.IsNullOrEmpty(value)
            ? string.Empty
            : $"\"{value.Replace("\"", "\"\"", StringComparison.Ordinal)}\"";

    private static string? Clean(string? value) =>
        string.IsNullOrWhiteSpace(value) ? null : value.Trim();

    private static CrewHoursResponse ToResponse(CrewHoursRow row) =>
        new(
            row.Id,
            row.CrewMemberId,
            row.WorkDate,
            row.Hours,
            row.Source,
            row.FunctionId,
            row.Status,
            row.Remark
        );
}
#pragma warning restore CS1591, MA0004
