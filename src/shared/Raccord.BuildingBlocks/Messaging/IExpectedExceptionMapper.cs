namespace Raccord.BuildingBlocks.Messaging;

/// <summary>Maps only known expected exceptions into business failures.</summary>
public interface IExpectedExceptionMapper
{
    /// <summary>Returns a mapping, or null when the exception is not owned by this mapper.</summary>
    ExpectedFailure? TryMap(Exception exception);
}
