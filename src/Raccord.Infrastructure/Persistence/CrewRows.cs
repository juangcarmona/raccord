using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Raccord.Contracts.Crew;

namespace Raccord.Infrastructure.Persistence;

#pragma warning disable MA0048

internal sealed class CrewMemberRow
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? LastName { get; set; }
    public string? Folder { get; set; }
    public string Skills { get; set; } = "[]";
    public bool IsPlannable { get; set; }
}

internal sealed class VehicleRow
{
    public Guid Id { get; set; }
    public string RegistrationNumber { get; set; } = string.Empty;
    public int Seats { get; set; }
    public string? Folder { get; set; }
    public string Tags { get; set; } = "[]";
    public bool IsShownInPlanning { get; set; }
}

internal sealed class CrewRateRow
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public CrewRateKind Kind { get; set; }
    public string Bands { get; set; } = "[]";
    public bool IsDefault { get; set; }
}

internal static class CrewJson
{
    public static string Serialize<T>(IReadOnlyList<T>? values) =>
        JsonSerializer.Serialize(values ?? []);

    public static IReadOnlyList<T> Deserialize<T>(string value) =>
        JsonSerializer.Deserialize<List<T>>(value) ?? [];
}

internal sealed class CrewMemberConfiguration : IEntityTypeConfiguration<CrewMemberRow>
{
    public void Configure(EntityTypeBuilder<CrewMemberRow> builder)
    {
        builder.ToTable("crew_members", "crew");
        builder.HasKey(row => row.Id);
        builder.Property(row => row.FirstName).HasMaxLength(100).IsRequired();
        builder.Property(row => row.Role).HasMaxLength(100).IsRequired();
        builder.Property(row => row.Email).HasMaxLength(320).IsRequired();
        builder.Property(row => row.LastName).HasMaxLength(100);
        builder.Property(row => row.Folder).HasMaxLength(100);
        builder.Property(row => row.Skills).HasColumnType("jsonb");
        builder.HasIndex(row => row.Email).IsUnique();
    }
}

internal sealed class VehicleConfiguration : IEntityTypeConfiguration<VehicleRow>
{
    public void Configure(EntityTypeBuilder<VehicleRow> builder)
    {
        builder.ToTable("vehicles", "crew");
        builder.HasKey(row => row.Id);
        builder.Property(row => row.RegistrationNumber).HasMaxLength(32).IsRequired();
        builder.Property(row => row.Folder).HasMaxLength(100);
        builder.Property(row => row.Tags).HasColumnType("jsonb");
        builder.HasIndex(row => row.RegistrationNumber).IsUnique();
    }
}

internal sealed class CrewRateConfiguration : IEntityTypeConfiguration<CrewRateRow>
{
    public void Configure(EntityTypeBuilder<CrewRateRow> builder)
    {
        builder.ToTable("crew_rates", "crew");
        builder.HasKey(row => row.Id);
        builder.Property(row => row.Name).HasMaxLength(160).IsRequired();
        builder.Property(row => row.Kind).HasConversion<string>().HasMaxLength(16);
        builder.Property(row => row.Bands).HasColumnType("jsonb");
        builder.HasIndex(row => new { row.Name, row.Kind }).IsUnique();
    }
}
