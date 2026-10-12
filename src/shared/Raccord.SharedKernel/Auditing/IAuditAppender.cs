namespace Raccord.SharedKernel.Auditing;

/// <summary>Append-only port for adding audit facts to the current unit of work.</summary>
public interface IAuditAppender
{
    /// <summary>Appends an audit fact without committing the current unit of work.</summary>
    void Append(AuditEntry entry);
}
