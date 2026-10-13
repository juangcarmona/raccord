namespace Raccord.Infrastructure.Persistence;

internal sealed class WorkspacePreferencesRow
{
    public int Id { get; set; }
    public required string Language { get; set; }
    public required string TimeZone { get; set; }
}
