using System.Net.Mail;
using Microsoft.EntityFrameworkCore;
using Raccord.Contracts.Crew;

namespace Raccord.Infrastructure.Persistence;

#pragma warning disable CS1591, MA0004, MA0048, MA0051

public sealed class CrewCatalog(RaccordDbContext db)
{
    public async Task<IReadOnlyList<CrewMemberResponse>> ListCrewAsync(CancellationToken ct) =>
        (await db.CrewMembers.AsNoTracking().OrderBy(row => row.FirstName).ToListAsync(ct))
            .Select(ToCrew)
            .ToArray();

    public async Task<CrewMemberResponse?> CreateCrewAsync(
        CreateCrewMemberRequest request,
        CancellationToken ct
    )
    {
        var email = request.Email.Trim();
        if (
            !ValidCrew(request.FirstName, request.Role, email)
            || await db.CrewMembers.AnyAsync(row => row.Email == email, ct)
        )
            return null;
        var row = new CrewMemberRow
        {
            Id = Guid.NewGuid(),
            FirstName = request.FirstName.Trim(),
            Role = request.Role.Trim(),
            Email = email,
            LastName = Clean(request.LastName),
            Folder = Clean(request.Folder),
            Skills = CrewJson.Serialize(request.Skills),
            IsPlannable = request.IsPlannable,
        };
        db.CrewMembers.Add(row);
        await db.SaveChangesAsync(ct);
        return ToCrew(row);
    }

    public async Task<CrewMemberResponse?> UpdateCrewAsync(
        Guid id,
        UpdateCrewMemberRequest request,
        CancellationToken ct
    )
    {
        var email = request.Email.Trim();
        var row = await db.CrewMembers.FirstOrDefaultAsync(candidate => candidate.Id == id, ct);
        if (
            row is null
            || !ValidCrew(request.FirstName, request.Role, email)
            || await db.CrewMembers.AnyAsync(
                candidate => candidate.Id != id && candidate.Email == email,
                ct
            )
        )
            return null;
        row.FirstName = request.FirstName.Trim();
        row.Role = request.Role.Trim();
        row.Email = email;
        row.LastName = Clean(request.LastName);
        row.Folder = Clean(request.Folder);
        row.Skills = CrewJson.Serialize(request.Skills);
        row.IsPlannable = request.IsPlannable;
        await db.SaveChangesAsync(ct);
        return ToCrew(row);
    }

    public async Task<IReadOnlyList<VehicleResponse>> ListVehiclesAsync(CancellationToken ct) =>
        (await db.Vehicles.AsNoTracking().OrderBy(row => row.RegistrationNumber).ToListAsync(ct))
            .Select(ToVehicle)
            .ToArray();

    public async Task<VehicleResponse?> CreateVehicleAsync(
        CreateVehicleRequest request,
        CancellationToken ct
    )
    {
        var registration = request.RegistrationNumber.Trim();
        if (
            !ValidVehicle(registration, request.Seats)
            || await db.Vehicles.AnyAsync(row => row.RegistrationNumber == registration, ct)
        )
            return null;
        var row = new VehicleRow
        {
            Id = Guid.NewGuid(),
            RegistrationNumber = registration,
            Seats = request.Seats,
            Folder = Clean(request.Folder),
            Tags = CrewJson.Serialize(request.Tags),
            IsShownInPlanning = request.IsShownInPlanning,
        };
        db.Vehicles.Add(row);
        await db.SaveChangesAsync(ct);
        return ToVehicle(row);
    }

    public async Task<VehicleResponse?> UpdateVehicleAsync(
        Guid id,
        UpdateVehicleRequest request,
        CancellationToken ct
    )
    {
        var registration = request.RegistrationNumber.Trim();
        var row = await db.Vehicles.FirstOrDefaultAsync(candidate => candidate.Id == id, ct);
        if (
            row is null
            || !ValidVehicle(registration, request.Seats)
            || await db.Vehicles.AnyAsync(
                candidate => candidate.Id != id && candidate.RegistrationNumber == registration,
                ct
            )
        )
            return null;
        row.RegistrationNumber = registration;
        row.Seats = request.Seats;
        row.Folder = Clean(request.Folder);
        row.Tags = CrewJson.Serialize(request.Tags);
        row.IsShownInPlanning = request.IsShownInPlanning;
        await db.SaveChangesAsync(ct);
        return ToVehicle(row);
    }

    public async Task<IReadOnlyList<CrewRateResponse>> ListRatesAsync(CancellationToken ct) =>
        (await db.CrewRates.AsNoTracking().OrderBy(row => row.Name).ToListAsync(ct))
            .Select(ToRate)
            .ToArray();

    public async Task<CrewRateResponse?> CreateRateAsync(
        CreateCrewRateRequest request,
        CancellationToken ct
    )
    {
        if (
            !ValidRate(request)
            || await db.CrewRates.AnyAsync(
                row => row.Name == request.Name.Trim() && row.Kind == request.Kind,
                ct
            )
        )
            return null;
        if (request.IsDefault)
            await db
                .CrewRates.Where(row => row.Kind == request.Kind && row.IsDefault)
                .ExecuteUpdateAsync(
                    setters => setters.SetProperty(row => row.IsDefault, false),
                    ct
                );
        var row = new CrewRateRow
        {
            Id = Guid.NewGuid(),
            Name = request.Name.Trim(),
            Kind = request.Kind,
            Bands = CrewJson.Serialize(request.Bands),
            IsDefault = request.IsDefault,
        };
        db.CrewRates.Add(row);
        await db.SaveChangesAsync(ct);
        return ToRate(row);
    }

    private static bool ValidCrew(string first, string role, string email) =>
        !string.IsNullOrWhiteSpace(first)
        && !string.IsNullOrWhiteSpace(role)
        && MailAddress.TryCreate(email, out _);

    private static bool ValidVehicle(string registration, int seats) =>
        !string.IsNullOrWhiteSpace(registration) && seats > 0;

    private static bool ValidRate(CreateCrewRateRequest request) =>
        !string.IsNullOrWhiteSpace(request.Name)
        && request.Bands.Count > 0
        && request.Bands.All(b => b.FromHour >= 0 && b.FixedAmount >= 0 && b.HourlyAmount >= 0)
        && request
            .Bands.OrderBy(b => b.FromHour)
            .Select(b => b.FromHour)
            .SequenceEqual(request.Bands.Select(b => b.FromHour));

    private static string? Clean(string? value) =>
        string.IsNullOrWhiteSpace(value) ? null : value.Trim();

    private static CrewMemberResponse ToCrew(CrewMemberRow row) =>
        new(
            row.Id,
            row.FirstName,
            row.Role,
            row.Email,
            row.LastName,
            row.Folder,
            CrewJson.Deserialize<string>(row.Skills),
            row.IsPlannable
        );

    private static VehicleResponse ToVehicle(VehicleRow row) =>
        new(
            row.Id,
            row.RegistrationNumber,
            row.Seats,
            row.Folder,
            CrewJson.Deserialize<string>(row.Tags),
            row.IsShownInPlanning
        );

    private static CrewRateResponse ToRate(CrewRateRow row) =>
        new(
            row.Id,
            row.Name,
            row.Kind,
            CrewJson.Deserialize<CrewRateBand>(row.Bands),
            row.IsDefault
        );
}
